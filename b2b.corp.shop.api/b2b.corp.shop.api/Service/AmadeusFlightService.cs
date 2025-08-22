using b2b.corp.shop.api.Models.Amadeus;
using b2b.corp.shop.api.Models.Api;
using b2b.corp.shop.api.Models.Common;
using b2b.corp.shop.api.Transformers;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Tzy.Flight.Api.Validators;

namespace b2b.corp.shop.api.Service
{
    /// <summary>
    /// Service to interact with Amadeus API for OAuth token and flight offers.
    /// </summary>
    public class AmadeusFlightService
    {
        private readonly HttpClient _httpClient;
        
        private string _cachedAccessToken;
        private DateTime _tokenExpiryUtc;
        private readonly SemaphoreSlim _tokenLock = new(1, 1);

        // Ideally read from secure config
        private readonly string _clientId = "WbpvP9dxvs0XNeSBrj62ulCAi1ATGGKe";
        private readonly string _clientSecret = "CKzhPIWW3GH4ET7l";
        private readonly string _tokenEndpoint = "https://test.api.amadeus.com/v1/security/oauth2/token";
        private readonly string _flightSearchEndpoint = "https://test.api.amadeus.com/v2/shopping/flight-offers";

        public AmadeusFlightService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_supabaseRepo = supabaseRepo;
        }

        private async Task<string> GetOrCachedAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedAccessToken) && DateTime.UtcNow < _tokenExpiryUtc)
                return _cachedAccessToken;

            await _tokenLock.WaitAsync();
            try
            {
                if (!string.IsNullOrEmpty(_cachedAccessToken) && DateTime.UtcNow < _tokenExpiryUtc)
                    return _cachedAccessToken;

                var formData = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", _clientId),
                    new("client_secret", _clientSecret)
                };

                var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint)
                {
                    Content = new FormUrlEncodedContent(formData)
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to get token. Status: {response.StatusCode}, Body: {error}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                    throw new Exception("Access token not found in response.");

                _cachedAccessToken = tokenResponse.AccessToken;
                _tokenExpiryUtc = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60); // Subtract 60s for safety
                return _cachedAccessToken;
            }
            finally
            {
                _tokenLock.Release();
            }
        }

        /// <summary>
        /// Searches for flight offers using Amadeus API.
        /// </summary>
        /// <param name="request">Structured request object with flight search criteria.</param>
        /// <returns>FlightOfferSearchResponse containing matching flights.</returns>
        public async Task<ApiBaseResponse<FlightListingApiResponse>> GetFlightOffersAsync(FlightListingApiRequest listing)
        {
            var validator = new FlightListingRequestValidator();
            FluentValidation.Results.ValidationResult? validationResult = validator.Validate(listing);
            string transactionId = Guid.NewGuid().ToString();
            if (!validationResult.IsValid)
            {
                return ApiBaseResponse<FlightListingApiResponse>.CreateError(transactionId, 400, "FlightListingApiRequest validation failed");
            }

            var accessToken = await GetOrCachedAccessTokenAsync();
            FlightOfferSearchRequest request = listing.ToAmadeusRequest();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var requestJson = JsonSerializer.Serialize(request, jsonOptions);
            Console.WriteLine($"[DEBUG] Request JSON: {requestJson}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _flightSearchEndpoint)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/vnd.amadeus+json")
            };

            httpRequest.Headers.Add("X-HTTP-Method-Override", "GET");
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.amadeus+json"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                var response = await _httpClient.SendAsync(httpRequest);
                var content = await response.Content.ReadAsStringAsync();
                FlightListingApiResponse apiResponse;
                if (!response.IsSuccessStatusCode)
                {
                    apiResponse = new FlightListingApiResponse
                    {
                        Success = false,
                        Message = $"Supplier API error: {response.StatusCode} - {content}",
                        TripType = listing.TripType,
                        Adults = listing.Adults,
                        Children = listing.Children,
                        Infants = listing.Infants,
                        Flights = new List<FlightOption>()
                    };
                    return ApiBaseResponse<FlightListingApiResponse>.CreateError(transactionId, (long)response.StatusCode, apiResponse.Message);
                }
                else
                {
                    var supplierResponseActual = JsonSerializer.Deserialize<FlightOfferSearchResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new FlightOfferSearchResponse { Data = new List<FlightOffer>() };

                    apiResponse = supplierResponseActual.ToApiResponse(listing.TripType, listing.Adults, listing.Children, listing.Infants);
                    apiResponse.Success = true;
                    apiResponse.Message = "Success";
                    return ApiBaseResponse<FlightListingApiResponse>.CreateSuccess(apiResponse, transactionId);
                }
            }
            catch (Exception ex)
            {
                var apiResponse = new FlightListingApiResponse
                {
                    Success = false,
                    Message = $"Supplier API exception: {ex.Message}",
                    TripType = listing.TripType,
                    Adults = listing.Adults,
                    Children = listing.Children,
                    Infants = listing.Infants,
                    Flights = new List<FlightOption>()
                };
                return ApiBaseResponse<FlightListingApiResponse>.CreateError(transactionId, 500, apiResponse.Message);
            }
        }
    }
}
