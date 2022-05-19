using System.Net.Http.Json;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class NegativeEventApi: INegativeEventApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public NegativeEventApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<NegativeGameEventResult>>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "negative_events";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<NegativeGameEventResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<NegativeGameEventResult>>();
            }

            return result;
        }

        public async Task<NegativeGameEventResult> EditAsync(NegativeGameEventRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<NegativeGameEventResult> DeleteAsync(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
