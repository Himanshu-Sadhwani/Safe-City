using MediatR;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.FieldReport;
using SafeCity.Events.FieldReport;
using SafeCity.Repository.Patrol;
using SafeCity.Utility;
using FieldReportEntity = SafeCity.Domain.Entity.FieldReport;
using IFieldReportRepository = SafeCity.Repository.FieldReport.IFieldReportRepository;

namespace SafeCity.Services.FieldReport
{
    /// <summary>Handles field report creation and updates with validation, persistence, and event publishing.</summary>
    public class FieldReportService : IFieldReportService
    {
        private readonly IFieldReportRepository _fieldReportRepository;
        private readonly IPatrolRepository _patrolRepository;
        private readonly IMediator _mediator;

        public FieldReportService(
            IFieldReportRepository fieldReportRepository,
            IPatrolRepository patrolRepository,
            IMediator mediator)
        {
            _fieldReportRepository = fieldReportRepository;
            _patrolRepository      = patrolRepository;
            _mediator              = mediator;
        }

        public async Task<FieldReportResponseDto> CreateAsync(CreateFieldReportDto dto, int officerId)
        {
            bool isDuplicate = await _fieldReportRepository.ExistsAsync(
                dto.PatrolId, dto.Notes, dto.Date!.Value);

            if (isDuplicate)
                throw new ConflictException(ErrorMessages.FieldReport.DuplicateReport);

            var patrol = await _patrolRepository.GetByIdAsync(dto.PatrolId);

            if (patrol is null)
                throw new NotFoundException(ErrorMessages.FieldReport.PatrolNotFound);

            if (patrol.OfficerId != officerId)
                throw new ForbiddenException(ErrorMessages.FieldReport.UnauthorizedOfficer);

            var entity = new FieldReportEntity
            {
                PatrolId = dto.PatrolId,
                Notes    = dto.Notes,
                Date     = dto.Date.Value,
                Status   = FieldReportStatus.Draft
            };

            await _fieldReportRepository.SaveAsync(entity);

            await _mediator.Publish(new FieldReportCreatedEvent(entity.ReportId, entity.PatrolId, entity.Date));

            return MapToResponse(entity);
        }
        
        public async Task<FieldReportResponseDto> UpdateAsync(int reportId, UpdateFieldReportDto dto, int officerId)
        {
            var report = await _fieldReportRepository.GetByIdAsync(reportId);

            if (report is null)
                throw new NotFoundException(ErrorMessages.FieldReport.ReportNotFound);

            var patrol = await _patrolRepository.GetByIdAsync(report.PatrolId);

            if (patrol is null || patrol.OfficerId != officerId)
                throw new ForbiddenException(ErrorMessages.FieldReport.UnauthorizedOfficer);

            if (dto.Notes is not null)
                report.Notes = dto.Notes;

            if (dto.Status.HasValue)
                report.Status = dto.Status.Value;

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
