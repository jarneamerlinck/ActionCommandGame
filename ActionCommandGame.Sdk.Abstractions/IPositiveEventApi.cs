using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IPositiveEventApi
    {
        Task<ServiceResult<IList<PositiveGameEventResult>>> FindAsync();
        Task<ServiceResult<PositiveGameEventResult>> EditAsync( int id, PositiveGameEventRequest request);
        Task<ServiceResult<PositiveGameEventResult>> CreateAsync(PositiveGameEventRequest request);
        Task<ServiceResult<bool>> DeleteAsync(int eventId);
    } 
}
