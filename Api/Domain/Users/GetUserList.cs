using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetUserList : IRequest<List<User>> { }

public class GetUserListHandler : IRequestHandler<GetUserList, List<User>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserListHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<User>> Handle(GetUserList request, CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        return _mapper.Map<List<User>>(users);
    }
}
