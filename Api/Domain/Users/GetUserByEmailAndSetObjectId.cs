using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Helpers;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Attributes;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.Users;

[AllowedAuthorizationRole(AuthorizationRole.Administrator, AuthorizationRole.User)]
public class GetUserByEmailAndSetObjectId : IRequest<User?>
{
    [Sensitive]
    public ClaimsPrincipal ClaimsPrincipal { get; set; } = null!;
    public Guid? AzureAdObjectId { get; set; }
}

public class GetUserByEmailAndSetObjectIdHandler
    : IRequestHandler<GetUserByEmailAndSetObjectId, User?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByEmailAndSetObjectIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User?> Handle(
        GetUserByEmailAndSetObjectId request,
        CancellationToken cancellationToken
    )
    {
        var email = request.ClaimsPrincipal.GetEmail();

        if (string.IsNullOrWhiteSpace(email))
            return null;

        var user = await _context
            .Users.Where(user => user.Email == email)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return null;

        if (request.AzureAdObjectId.HasValue)
            user.AzureAdObjectId = (Guid)request.AzureAdObjectId;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<User>(user);
    }
}
