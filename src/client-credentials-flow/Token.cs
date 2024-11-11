using System.Text.Json.Serialization;

namespace client_credentials_flow;

internal sealed record Token
{
    [JsonPropertyName("access_token")] 
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; }
}