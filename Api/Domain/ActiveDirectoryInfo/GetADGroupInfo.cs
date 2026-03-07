using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ActiveDirectoryInfo;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetAdGroupInfo : IRequest<AdGroupInfo?>
{
    public string GroupName { get; set; } = null!;
}

public class GetAdGroupInfoHandler : IRequestHandler<GetAdGroupInfo, AdGroupInfo?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly AzureAdHelper _azureAdHelper;

    public GetAdGroupInfoHandler(AppDbContext context, IMapper mapper, AzureAdHelper azureAdHelper)
    {
        _context = context;
        _mapper = mapper;
        _azureAdHelper = azureAdHelper;
    }

    public async Task<AdGroupInfo?> Handle(
        GetAdGroupInfo request,
        CancellationToken cancellationToken
    )
    {
        var adGroupInfo = await _azureAdHelper.GetAdGroupInfoByGroupNameAsync(request.GroupName);

        return (adGroupInfo);
    }
}
