using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class CreateUser : IRequest<User>
{
    public User UserToCreate { get; set; } = null!;
}

public class CreateUserHandler : IRequestHandler<CreateUser, User>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateUserHandler(AppDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<User> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var currentLoggedinUser = await _mediator.Send(new GetCurrentUser(), cancellationToken);
        var user = _mapper.Map<Data.Models.User>(request.UserToCreate);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<User>(user);
    }
}
