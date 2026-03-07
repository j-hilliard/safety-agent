using AutoMapper;
using MediatR;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Api.Models;
using Stronghold.AppDashboard.Api.Services;
using Stronghold.AppDashboard.Data;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Domain.ActiveDirectoryInfo;

[AllowedAuthorizationRole(AuthorizationRole.Administrator)]
public class GetAdUserInfo : IRequest<AdUserInfo?>
{
    public Guid UserAzureAdObjectId { get; set; }
}

public class GetAdUserInfoHandler : IRequestHandler<GetAdUserInfo, AdUserInfo?>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly AzureAdHelper _azureAdHelper;

    public GetAdUserInfoHandler(AppDbContext context, IMapper mapper, AzureAdHelper azureAdHelper)
    {
        _context = context;
        _mapper = mapper;
        _azureAdHelper = azureAdHelper;
    }

    public async Task<AdUserInfo?> Handle(
        GetAdUserInfo request,
        CancellationToken cancellationToken
    )
    {
        var adUserInfo = await _azureAdHelper.GetAdUserInfoByObjectIdAsync(
            request.UserAzureAdObjectId
        );

        return (adUserInfo);
    }
}
