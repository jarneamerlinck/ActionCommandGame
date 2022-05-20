using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<ServiceResult<PositiveGameEventResult>> GetRandomPositiveGameEvent(bool hasAttackItem, string authenticatedUserId);
        Task<ServiceResult<IList<PositiveGameEventResult>>> FindAsync(string authenticatedUserId);
        Task<ServiceResult<PositiveGameEventResult>> EditAsync(int id, PositiveGameEventRequest request, string authenticatedUserId);
        Task<ServiceResult<PositiveGameEventResult>> CreateAsync(PositiveGameEventRequest request, string authenticatedUserId);
        Task<ServiceResult<bool>> DeleteAsync(int id, string authenticatedUserId);
    }
}
