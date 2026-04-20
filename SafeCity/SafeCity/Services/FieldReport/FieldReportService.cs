using MediatR;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.FieldReport;
using SafeCity.Events;
using SafeCity.Repository.Patrol;
using SafeCity.Utility;
using FieldReportEntity = SafeCity.Domain.Entity.FieldReport;
using IFieldReportRepository = SafeCity.Repository.FieldReport.IFieldReportRepository;

namespace SafeCity.Services.FieldReport
{
    /// <summary>
    /// Implements field report business logic: duplicate detection, patrol ownership validation,
    /// persistence, domain event publishing, and partial updates.
    /// </summary>
    public class FieldReportService : IFieldReportService
    {
        private readonly IFieldReportRepository _fieldReportRepository;
        private readonly IPatrolRepository _patrolRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of <see cref="FieldReportService"/>.
        /// </summary>
        public FieldReportService(
            IFieldReportRepository fieldReportRepository,
            IPatrolRepository patrolRepository,
            IMediator mediator)
        {
            _fieldReportRepository = fieldReportRepository;
            _patrolRepository      = patrolRepository;
            _mediator              = mediator;
        }

        /// <summary>
        /// Creates a new field report in the following order:
        /// <list type="number">
        ///   <item>Checks for a duplicate report; throws <see cref="ConflictException"/> if one exists.</item>
        ///   <item>Verifies the patrol exists; throws <see cref="NotFoundException"/> if not found.</item>
        ///   <item>Verifies the requesting officer owns the patrol; throws <see cref="ForbiddenException"/> if not.</item>
        ///   <item>Maps the DTO to a <see cref="FieldReportEntity"/> with status <c>Draft</c>.</item>
        ///   <item>Persists the entity.</item>
        ///   <item>Publishes a <see cref="FieldReportCreatedEvent"/> via MediatR.</item>
        ///   <item>Returns a <see cref="FieldReportResponseDto"/>.</item>
        /// </list>
        /// </summary>
        public async Task<FieldReportResponseDto> CreateAsync(CreateFieldReportDto dto, int officerId)
        {
            // 1. Duplicate check
            bool isDuplicate = await _fieldReportRepository.ExistsAsync(
                dto.PatrolId, dto.Notes, dto.Date!.Value);

            if (isDuplicate)
                throw new ConflictException(ErrorMessages.FieldReport.DuplicateReport);

            // 2. Patrol existence check
            var patrol = await _patrolRepository.GetByIdAsync(dto.PatrolId);

            if (patrol is null)
                throw new NotFoundException(ErrorMessages.FieldReport.PatrolNotFound);

            // 3. Ownership check — the submitting officer must own the patrol
            if (patrol.OfficerId != officerId)
                throw new ForbiddenException(ErrorMessages.FieldReport.UnauthorizedOfficer);

            // 4. Map DTO → entity (Draft = initial pending state)
            var entity = new FieldReportEntity
            {
                PatrolId = dto.PatrolId,
                Notes    = dto.Notes,
                Date     = dto.Date.Value,
                Status   = FieldReportStatus.Draft
            };

            // 5. Persist
            await _fieldReportRepository.SaveAsync(entity);

            // 6. Publish domain event
            await _mediator.Publish(new FieldReportCreatedEvent(entity.ReportId, entity.PatrolId, entity.Date));

            // 7. Return response DTO
            return MapToResponse(entity);
        }

        /// <summary>
        /// Partially updates the notes and/or status of an existing report.
        /// Only non-null fields in <paramref name="dto"/> are applied.
        /// </summary>
        public async Task<FieldReportResponseDto> UpdateAsync(int reportId, UpdateFieldReportDto dto, int officerId)
        {
            // 1. Report existence check
            var report = await _fieldReportRepository.GetByIdAsync(reportId);

            if (report is null)
                throw new NotFoundException(ErrorMessages.FieldReport.ReportNotFound);

            // 2. Ownership check via the linked patrol
            var patrol = await _patrolRepository.GetByIdAsync(report.PatrolId);

            if (patrol is null || patrol.OfficerId != officerId)
                throw new ForbiddenException(ErrorMessages.FieldReport.UnauthorizedOfficer);

            // 3. Apply partial updates
            if (dto.Notes is not null)
                report.Notes = dto.Notes;

            if (dto.Status.HasValue)
                report.Status = dto.Status.Value;

            // 4. Persist
            await _fieldReportRepository.UpdateAsync(report);

            return MapToResponse(report);
        }

        private static FieldReportResponseDto MapToResponse(FieldReportEntity entity) =>
            new FieldReportResponseDto
            {
                ReportId = entity.ReportId,
                PatrolId = entity.PatrolId,
                Notes    = entity.Notes,
                Date     = entity.Date,
                Status   = entity.Status.ToString()
            };
    }
}
