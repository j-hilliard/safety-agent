using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class UpdateUser : IRequest<User?>
{
    public int UserId { get; set; }

    public User UserToUpdate { get; set; } = null!;
}

public class UpdateUserHandler : IRequestHandler<UpdateUser, User?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateUserHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<User?> Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(
            new object?[] { request.UserId },
            cancellationToken: cancellationToken
        );
        if (user == null)
            return null;

        _mapper.Map(request.UserToUpdate, user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<User>(user);
    }
}
