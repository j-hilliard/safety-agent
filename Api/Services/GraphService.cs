using System.Net.Http.Headers;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Stronghold.AppDashboard.Api.Authorization;
using Stronghold.AppDashboard.Shared.Enumerations;

namespace Stronghold.AppDashboard.Api.Services;

[AllowedAuthorizationRole(AuthorizationRole.User)]
public class GraphService
{
    private readonly IPublicClientApplication _publicClientApp;

    public GraphService(IPublicClientApplication publicClientApp)
    {
        _publicClientApp = publicClientApp;
    }

    public async Task<string> GetTokenAsync(string[] scopes)
    {
        var accounts = await _publicClientApp.GetAccountsAsync();
        var firstAccount = accounts.FirstOrDefault();

        try
        {
            var authResult = await _publicClientApp.AcquireTokenInteractive(scopes).ExecuteAsync();

            return authResult.AccessToken;
        }
        catch (MsalUiRequiredException e)
        {
            return null;
        }
    }

    public async Task<byte[]> GetUserProfilePictureAsync(string accessToken)
    {
        var graphClient = new GraphServiceClient(
            new DelegateAuthenticationProvider(requestMessage =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );
                return Task.CompletedTask;
            })
        );

        var userPhotoStream = await graphClient.Me.Photo.Content.Request().GetAsync();

        using (var memoryStream = new MemoryStream())
        {
            await userPhotoStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
