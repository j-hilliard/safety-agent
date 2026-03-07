using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetActiveUsers : IRequest<List<User>> { }

public class GetActiveUsersHandler : IRequestHandler<GetActiveUsers, List<User>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetActiveUsersHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<User>> Handle(
        GetActiveUsers request,
        CancellationToken cancellationToken
    )
    {
        var users = await _context.Users.Where(user => user.Active).ToListAsync(cancellationToken);

        return _mapper.Map<List<User>>(users);
    }
}
