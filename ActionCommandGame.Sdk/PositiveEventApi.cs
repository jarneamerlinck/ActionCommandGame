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
    public class PositiveEventApi: IPositiveEventApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PositiveEventApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<PositiveGameEventResult>>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "positive_events";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<PositiveGameEventResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<PositiveGameEventResult>>();
            }

            return result;
        }

        public async Task<ServiceResult<PositiveGameEventResult>> EditAsync(int id, PositiveGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"positive_events/{id}";
            var data = JsonConvert.SerializeObject(request);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PositiveGameEventResult>>();

            if (result is null)
            {
                return new ServiceResult<PositiveGameEventResult>();
            }

            return result;
        }
        public async Task<ServiceResult<PositiveGameEventResult>> CreateAsync(PositiveGameEventRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"positive_events/";
            var data = JsonConvert.SerializeObject(request);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PositiveGameEventResult>>();

            if (result is null)
            {
                return new ServiceResult<PositiveGameEventResult>();
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"positive_events/Delete/{id}";
            

            var httpResponse = await httpClient.PostAsync(route, null);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<bool>>();

            return new ServiceResult<bool>(result is not null? result.IsSuccess:false);
        }
    }
}
