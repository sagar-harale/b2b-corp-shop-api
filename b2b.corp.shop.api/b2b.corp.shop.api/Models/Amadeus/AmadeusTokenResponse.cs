using System.Text.Json.Serialization;

namespace b2b.corp.shop.api.Models.Amadeus
{
    /// <summary>
    /// Represents Amadeus OAuth2 token response.
    /// </summary>
    public class AmadeusTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("application_name")]
        public string ApplicationName { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
