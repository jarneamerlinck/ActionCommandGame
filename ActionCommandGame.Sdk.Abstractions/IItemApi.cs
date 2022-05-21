using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IItemApi
    {
        Task<ServiceResult<IList<ItemResult>>> FindAsync();
        Task<ServiceResult<ItemResult>> EditAsync(int id, ItemRequest request);
        Task<ServiceResult<ItemResult>> CreateAsync(ItemRequest request);
        Task<ServiceResult<bool>> DeleteAsync(int itemId);
    }
}
