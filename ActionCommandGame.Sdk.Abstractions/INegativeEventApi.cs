using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface INegativeEventApi
    {
        Task<ServiceResult<IList<NegativeGameEventResult>>> FindAsync();
        Task<ServiceResult<NegativeGameEventResult>> EditAsync( int id, NegativeGameEventRequest request);
        Task<ServiceResult<NegativeGameEventResult>> CreateAsync(NegativeGameEventRequest request);
        Task<ServiceResult<bool>> DeleteAsync(int eventId);
    } 
}
