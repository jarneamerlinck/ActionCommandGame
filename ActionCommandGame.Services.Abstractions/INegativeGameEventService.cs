using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        Task<ServiceResult<NegativeGameEventResult>> GetRandomNegativeGameEvent(string authenticatedUserId);
        Task<ServiceResult<IList<NegativeGameEventResult>>> FindAsync(string authenticatedUserId);
        Task<ServiceResult<NegativeGameEventResult>> EditAsync(int id, NegativeGameEventRequest request, string authenticatedUserId);
        Task<ServiceResult<NegativeGameEventResult>> CreateAsync( NegativeGameEventRequest request, string authenticatedUserId);
        Task<ServiceResult<bool>> DeleteAsync(int id, string authenticatedUserId);
    }
}
