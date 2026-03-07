using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class EnableUser : IRequest<bool?>
{
    public int UserId { get; set; }
}

public class ActivateUserHandler : IRequestHandler<EnableUser, bool?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ActivateUserHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<bool?> Handle(EnableUser request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            return null;

        user.Active = true;
        user.DisabledOn = null;
        user.DisabledReason = null;
        user.DisabledById = null;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
