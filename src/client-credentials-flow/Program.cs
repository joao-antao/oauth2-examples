using System.Net;
using System.Net.Mime;
using client_credentials_flow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

Console.WriteLine("OAuth2 Client Credentials Flow");

var serviceProvider = new ServiceCollection()
    .AddTransient<OAuthMessageHandler>()
    .AddLogging(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Warning);
    })
    .AddHttpClient("spotify", (provider, httpClient) =>
    {
        httpClient.BaseAddress = new Uri("https://api.spotify.com/");
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, "deflate, gzip");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
    })
    .AddHttpMessageHandler<OAuthMessageHandler>()
    .Services.BuildServiceProvider();
    
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
var httpClient = httpClientFactory.CreateClient("spotify");

// Execute a request to a protected resource using the oauth2 client credentials flow
var response = await httpClient.GetAsync("v1/artists/0kbYTNQb4Pb1rPbbaF0pT4");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);