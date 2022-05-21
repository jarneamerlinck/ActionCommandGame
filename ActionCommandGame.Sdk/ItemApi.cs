using System.Net.Http.Json;
using System.Text;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Newtonsoft.Json;

namespace ActionCommandGame.Sdk
{
    public class ItemApi: IItemApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public ItemApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<ItemResult>>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "items";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<ItemResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<ItemResult>>();
            }

            return result;
        }

        public async Task<ServiceResult<ItemResult>> EditAsync(int id, ItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"items/{id}";
            var data = JsonConvert.SerializeObject(request);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<ItemResult>>();

            if (result is null)
            {
                return new ServiceResult<ItemResult>();
            }

            return result;
        }
        public async Task<ServiceResult<ItemResult>> CreateAsync(ItemRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"items/";
            var data = JsonConvert.SerializeObject(request);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<ItemResult>>();

            if (result is null)
            {
                return new ServiceResult<ItemResult>();
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"items/Delete/{id}";


            var httpResponse = await httpClient.PostAsync(route, null);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<bool>>();

            return new ServiceResult<bool>(result is not null ? result.IsSuccess : false);
        }
    }
}
