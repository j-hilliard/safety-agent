using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ReferenceData;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class DeleteCompany : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteCompanyHandler : IRequestHandler<DeleteCompany, Unit>
{
    private readonly AppDbContext _context;

    public DeleteCompanyHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompany request, CancellationToken cancellationToken)
    {
        var entity = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Company {request.Id} not found.");

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
