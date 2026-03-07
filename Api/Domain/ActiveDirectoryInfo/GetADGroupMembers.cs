using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ActiveDirectoryInfo;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetAdGroupMembers : IRequest<List<AdUserInfo>>
{
    public Guid GroupAzureObjectSid { get; set; }
}

public class GetAdGroupMembersHandler : IRequestHandler<GetAdGroupMembers, List<AdUserInfo>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly AzureAdHelper _azureAdHelper;

    public GetAdGroupMembersHandler(
        AppDbContext context,
        IMapper mapper,
        AzureAdHelper azureAdHelper
    )
    {
        _context = context;
        _mapper = mapper;
        _azureAdHelper = azureAdHelper;
    }

    public async Task<List<AdUserInfo>> Handle(
        GetAdGroupMembers request,
        CancellationToken cancellationToken
    )
    {
        var adGroupMembers = await _azureAdHelper.GetAdGroupMembersByGroupObjectIdAsync(
            request.GroupAzureObjectSid
        );

        return (adGroupMembers);
    }
}
