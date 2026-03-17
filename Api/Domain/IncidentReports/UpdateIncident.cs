using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Data.Models.Safety;
using Stronghold.AppDashboard.Shared.Enumerations;
using IncidentReportDto = Stronghold.AppDashboard.Api.Models.IncidentReport;

namespace Stronghold.AppDashboard.Api.Domain.IncidentReports;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class UpdateIncident : IRequest<IncidentReportDto?>
{
    public Guid IncidentReportId { get; set; }
    public IncidentReportDto IncidentReportToUpdate { get; set; } = null!;
}

public class UpdateIncidentHandler : IRequestHandler<UpdateIncident, IncidentReportDto?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IProcessLogService _log;

    public UpdateIncidentHandler(AppDbContext context, IMapper mapper, IProcessLogService log)
    {
        _context = context;
        _mapper = mapper;
        _log = log;
    }

    public async Task<IncidentReportDto?> Handle(UpdateIncident request, CancellationToken cancellationToken)
    {
        var entity = await _context.IncidentReports
            .Include(r => r.EmployeesInvolved)
            .Include(r => r.Actions)
            .Include(r => r.References)
            .FirstOrDefaultAsync(r => r.Id == request.IncidentReportId, cancellationToken);

        if (entity == null)
            return null;

        try
        {
            var dto = request.IncidentReportToUpdate;

            // Update scalar fields
            entity.Status = dto.Status ?? entity.Status;
            entity.IncidentDate = dto.IncidentDate;
            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.JobNumber = dto.JobNumber;
            entity.ClientCode = dto.ClientCode;
            entity.PlantCode = dto.PlantCode;
            entity.WorkDescription = dto.WorkDescription;
            entity.IncidentSummary = dto.IncidentSummary;
            entity.IncidentClass = dto.IncidentClass;
            entity.SeverityActualCode = dto.SeverityActualCode;
            entity.SeverityPotentialCode = dto.SeverityPotentialCode;
            entity.HealthSafetyLeaderId = dto.HealthSafetyLeaderId;
            entity.SeniorOpsLeaderId = dto.SeniorOpsLeaderId;
            entity.UpdatedAt = DateTime.UtcNow;

            // Replace child collections
            _context.IncidentEmployeesInvolved.RemoveRange(entity.EmployeesInvolved);
            entity.EmployeesInvolved.Clear();
            foreach (var emp in dto.EmployeesInvolved)
            {
                var empEntity = _mapper.Map<IncidentEmployeeInvolved>(emp);
                empEntity.Id = emp.Id == Guid.Empty ? Guid.NewGuid() : emp.Id;
                empEntity.IncidentReportId = entity.Id;
                empEntity.CreatedAt = DateTime.UtcNow;
                empEntity.UpdatedAt = DateTime.UtcNow;
                entity.EmployeesInvolved.Add(empEntity);
            }

            _context.IncidentActions.RemoveRange(entity.Actions);
            entity.Actions.Clear();
            foreach (var action in dto.Actions)
            {
                var actionEntity = _mapper.Map<IncidentAction>(action);
                actionEntity.Id = action.Id == Guid.Empty ? Guid.NewGuid() : action.Id;
                actionEntity.IncidentReportId = entity.Id;
                actionEntity.CreatedAt = DateTime.UtcNow;
                actionEntity.UpdatedAt = DateTime.UtcNow;
                entity.Actions.Add(actionEntity);
            }

            _context.IncidentReportReferences.RemoveRange(entity.References);
            entity.References.Clear();
            foreach (var refId in dto.ReferenceIds)
            {
                entity.References.Add(new IncidentReportReference
                {
                    IncidentReportId = entity.Id,
                    ReferenceId = refId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _log.LogAsync("UpdateIncident", "IncidentReport", "Info",
                $"Incident report updated: {entity.IncidentNumber}", entity.Id, relatedObject: entity.IncidentNumber);

            var updated = await _context.IncidentReports
                .Include(r => r.Company)
                .Include(r => r.Region)
                .Include(r => r.EmployeesInvolved)
                .Include(r => r.Actions)
                .Include(r => r.References)
                .FirstAsync(r => r.Id == entity.Id, cancellationToken);

            return _mapper.Map<IncidentReportDto>(updated);
        }
        catch (Exception ex)
        {
            await _log.LogAsync("UpdateIncident", "IncidentReport", "Error",
                $"Failed to update incident report: {ex.Message}",
                entity.Id,
                messageDetail: ex.ToString(),
                relatedObject: entity.IncidentNumber);
            throw;
        }
    }
}
