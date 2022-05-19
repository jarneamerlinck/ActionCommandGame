using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface INegativeEventApi
    {
        Task<ServiceResult<IList<NegativeGameEventResult>>> FindAsync();
        Task<NegativeGameEventResult> EditAsync(NegativeGameEventRequest request);
        Task<NegativeGameEventResult> DeleteAsync(int eventId);
    } 
}
