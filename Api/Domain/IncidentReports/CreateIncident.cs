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
public class CreateIncident : IRequest<IncidentReportDto>
{
    public IncidentReportDto IncidentReportToCreate { get; set; } = null!;
}

public class CreateIncidentHandler : IRequestHandler<CreateIncident, IncidentReportDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IProcessLogService _log;

    public CreateIncidentHandler(AppDbContext context, IMapper mapper, IProcessLogService log)
    {
        _context = context;
        _mapper = mapper;
        _log = log;
    }

    public async Task<IncidentReportDto> Handle(CreateIncident request, CancellationToken cancellationToken)
    {
        var dto = request.IncidentReportToCreate;
        Guid? entityId = null;
        string incidentNumber = string.Empty;

        try
        {
            // Generate incident number atomically to prevent race conditions.
            // The UPDATE...OUTPUT pattern is a single atomic operation — SQL Server row-level
            // locking ensures no two concurrent requests can receive the same number.
            if (dto.CompanyId.HasValue)
            {
                var connection = _context.Database.GetDbConnection();
                var wasOpen = connection.State == System.Data.ConnectionState.Open;
                if (!wasOpen) await connection.OpenAsync(cancellationToken);

                try
                {
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                        UPDATE safety.SHC_Company
                        SET next_incident_number = next_incident_number + 1,
                            updatedAt = GETUTCDATE()
                        OUTPUT DELETED.next_incident_number, DELETED.code
                        WHERE company_id = @companyId";
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@companyId";
                    param.Value = dto.CompanyId.Value;
                    cmd.Parameters.Add(param);

                    using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                    if (!await reader.ReadAsync(cancellationToken))
                        throw new ArgumentException("Company not found.");

                    var nextNum = reader.GetInt32(0);
                    var code = reader.GetString(1);
                    incidentNumber = $"{code}-{DateTime.UtcNow.Year}-{nextNum:D4}";
                }
                finally
                {
                    if (!wasOpen) await connection.CloseAsync();
                }
            }
            else
            {
                incidentNumber = $"SHC-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
            }

            var entity = _mapper.Map<Data.Models.Safety.IncidentReport>(dto);
            entity.Id = Guid.NewGuid();
            entityId = entity.Id;
            entity.IncidentNumber = incidentNumber;
            entity.Status = entity.Status ?? "FIRSTREPORT";
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            // Map child collections
            foreach (var emp in dto.EmployeesInvolved)
            {
                var empEntity = _mapper.Map<IncidentEmployeeInvolved>(emp);
                empEntity.Id = Guid.NewGuid();
                empEntity.IncidentReportId = entity.Id;
                empEntity.CreatedAt = DateTime.UtcNow;
                empEntity.UpdatedAt = DateTime.UtcNow;
                entity.EmployeesInvolved.Add(empEntity);
            }

            foreach (var action in dto.Actions)
            {
                var actionEntity = _mapper.Map<IncidentAction>(action);
                actionEntity.Id = Guid.NewGuid();
                actionEntity.IncidentReportId = entity.Id;
                actionEntity.CreatedAt = DateTime.UtcNow;
                actionEntity.UpdatedAt = DateTime.UtcNow;
                entity.Actions.Add(actionEntity);
            }

            foreach (var refId in dto.ReferenceIds)
            {
                entity.References.Add(new IncidentReportReference
                {
                    IncidentReportId = entity.Id,
                    ReferenceId = refId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.IncidentReports.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _log.LogAsync("CreateIncident", "IncidentReport", "Info",
                $"Incident report created: {incidentNumber}", entity.Id, relatedObject: incidentNumber);

            var created = await _context.IncidentReports
                .Include(r => r.Company)
                .Include(r => r.Region)
                .Include(r => r.EmployeesInvolved)
                .Include(r => r.Actions)
                .Include(r => r.References)
                .FirstAsync(r => r.Id == entity.Id, cancellationToken);

            return _mapper.Map<IncidentReportDto>(created);
        }
        catch (Exception ex)
        {
            await _log.LogAsync("CreateIncident", "IncidentReport", "Error",
                $"Failed to create incident report: {ex.Message}",
                entityId,
                messageDetail: ex.ToString(),
                relatedObject: incidentNumber);
            throw;
        }
    }
}
