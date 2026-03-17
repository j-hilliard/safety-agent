using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class SaveCompany : IRequest<RefCompanyDto>
{
    public Guid? Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}

public class SaveCompanyHandler : IRequestHandler<SaveCompany, RefCompanyDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SaveCompanyHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RefCompanyDto> Handle(SaveCompany request, CancellationToken cancellationToken)
    {
        Data.Models.Safety.RefCompany entity;

        if (request.Id.HasValue)
        {
            entity = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == request.Id.Value, cancellationToken)
                ?? throw new KeyNotFoundException($"Company {request.Id} not found.");
        }
        else
        {
            entity = new Data.Models.Safety.RefCompany { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
            await _context.Companies.AddAsync(entity, cancellationToken);
        }

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RefCompanyDto>(entity);
    }
}
