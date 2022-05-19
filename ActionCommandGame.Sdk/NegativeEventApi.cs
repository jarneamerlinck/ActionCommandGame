using System.Net.Http.Json;
using System.Text;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Newtonsoft.Json;

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

        public async Task<ServiceResult<NegativeGameEventResult>> EditAsync(int id, NegativeGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"negative_events/{id}";
            var data = JsonConvert.SerializeObject(request);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<NegativeGameEventResult>>();

            if (result is null)
            {
                return new ServiceResult<NegativeGameEventResult>();
            }

            return result;
        }

        public async Task<ServiceResult<NegativeGameEventResult>> DeleteAsync(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
