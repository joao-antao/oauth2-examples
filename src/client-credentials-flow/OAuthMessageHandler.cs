using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace client_credentials_flow;

public sealed class OAuthMessageHandler : DelegatingHandler
{
    private const string Scheme = "Bearer";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetAccessTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue(Scheme, token.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
    
    private async Task<Token> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        var requestBody = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", "your_client_id"),
            new KeyValuePair<string, string>("client_secret", "your_client_secret"),
        ]);
        request.Content = requestBody;

        var response = await base.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Token>(cancellationToken) ?? throw new InvalidOperationException();
    }
}