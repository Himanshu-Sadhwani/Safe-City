using SafeCity.Domain.Enum;
using SafeCity.DTOs.FieldReport;
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
        public FieldReportService(
            IFieldReportRepository fieldReportRepository,
            IPatrolRepository patrolRepository
            )
        {
            _fieldReportRepository = fieldReportRepository;
            _patrolRepository      = patrolRepository;
        }

        public async Task<FieldReportResponseDto> CreateAsync(CreateFieldReportDto dto, int officerId)
        {
            bool isDuplicate = await _fieldReportRepository.ExistsAsync(
                dto.PatrolId!.Value, dto.Notes, dto.Date!.Value);

            if (isDuplicate)
                throw new ConflictException(ErrorMessages.FieldReport.DuplicateReport);

            var patrol = await _patrolRepository.GetByIdAsync(dto.PatrolId!.Value);

            if (patrol is null)
                throw new NotFoundException(ErrorMessages.FieldReport.PatrolNotFound);

            if (patrol.OfficerId != officerId)
                throw new ForbiddenException(ErrorMessages.FieldReport.UnauthorizedOfficer);

            var entity = new FieldReportEntity
            {
                PatrolId = dto.PatrolId!.Value,
                Notes    = dto.Notes,
                Date     = dto.Date.Value,
                Status   = FieldReportStatus.Draft
            };

            await _fieldReportRepository.SaveAsync(entity);

            return new FieldReportResponseDto
            {
                ReportId = entity.ReportId,
                PatrolId = entity.PatrolId,
                Notes    = entity.Notes,
                Date     = entity.Date,
                Status   = entity.Status.ToString()
            };
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
            {
                if (!Enum.IsDefined(typeof(FieldReportStatus), dto.Status.Value))
                    throw new ArgumentException(ErrorMessages.FieldReport.NosuchStatus);

                report.Status = dto.Status.Value;
            }

            await _fieldReportRepository.UpdateAsync(report);

            return new FieldReportResponseDto
            {
                ReportId = report.ReportId,
                PatrolId = report.PatrolId,
                Notes    = report.Notes,
                Date     = report.Date,
                Status   = report.Status.ToString()
            };
        }

        public async Task<IEnumerable<FieldReportResponseDto>> GetAllAsync(FieldReportFilterDto filter)
        {
            // Validate ReportId if provided
            if (filter.ReportId.HasValue)
            {
                var reportExists = await _fieldReportRepository.GetByIdAsync(filter.ReportId.Value);
                if (reportExists is null)
                    throw new NotFoundException("no such Report Id");
            }

            // Validate PatrolId if provided
            if (filter.PatrolId.HasValue)
            {
                var patrolExists = await _patrolRepository.GetByIdAsync(filter.PatrolId.Value);
                if (patrolExists is null)
                    throw new NotFoundException("no such Patrol Id");
            }

            var reports = await _fieldReportRepository.GetAllAsync(filter);

            if (!reports.Any())
            {
                if (filter.Status.HasValue)
                    throw new NotFoundException("no Field report under required status");

                if (filter.ReportId.HasValue || filter.PatrolId.HasValue || filter.Date.HasValue)
                    throw new NotFoundException("Field Report does not exist for required fields");
            }

            return reports.Select(f => new FieldReportResponseDto
            {
                ReportId = f.ReportId,
                PatrolId = f.PatrolId,
                Notes    = f.Notes,
                Date     = f.Date,
                Status   = f.Status.ToString()
            });
        }
    }
}
