using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class DisableUser : IRequest<bool?>
{
    public int UserId { get; set; }

    public string Reason { get; set; } = null!;
}

public class DisableUserHandler : IRequestHandler<DisableUser, bool?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public DisableUserHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<bool?> Handle(DisableUser request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            return null;

        var currentLoggedinUser = await _mediator.Send(new GetCurrentUser(), cancellationToken);
        if (currentLoggedinUser != null)
        {
            user.DisabledById = currentLoggedinUser.UserId;
        }

        user.Active = false;
        user.DisabledOn = DateTime.UtcNow;
        user.DisabledReason = request.Reason;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
