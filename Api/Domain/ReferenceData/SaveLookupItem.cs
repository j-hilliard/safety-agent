using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class SaveLookupItem : IRequest<RefOptionDto>
{
    public Guid? Id { get; set; }
    public Guid ReferenceTypeId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}

public class SaveLookupItemHandler : IRequestHandler<SaveLookupItem, RefOptionDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SaveLookupItemHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RefOptionDto> Handle(SaveLookupItem request, CancellationToken cancellationToken)
    {
        Data.Models.Safety.RefIncidentReportReference entity;

        if (request.Id.HasValue)
        {
            entity = await _context.IncidentReportReferenceOptions
                .Include(r => r.ReferenceType)
                .FirstOrDefaultAsync(r => r.Id == request.Id.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Lookup item {request.Id} not found.");
        }
        else
        {
            entity = new Data.Models.Safety.RefIncidentReportReference { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
            await _context.IncidentReportReferenceOptions.AddAsync(entity, cancellationToken);
        }

        entity.ReferenceTypeId = request.ReferenceTypeId;
        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        // Reload with navigation property for mapping
        await _context.Entry(entity).Reference(e => e.ReferenceType).LoadAsync(cancellationToken);
        return _mapper.Map<RefOptionDto>(entity);
    }
}
