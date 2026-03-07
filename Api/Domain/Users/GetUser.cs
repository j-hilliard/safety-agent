using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetUser : IRequest<User?>
{
    public int UserId { get; set; }
}

public class GetUserHandler : IRequestHandler<GetUser, User?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Handle(GetUser request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.UserId == request.UserId,
            cancellationToken
        );

        if (user == null)
        {
            return null;
        }

        User userResult = _mapper.Map<User>(user);

        var roles = await _context
            .UserRoles.Where(ur => ur.UserId == user.UserId)
            .Include(ur => ur.Role)
            .ToListAsync(cancellationToken);

        userResult.Roles = _mapper.Map<List<UserRole>>(roles);

        return userResult;
    }
}
