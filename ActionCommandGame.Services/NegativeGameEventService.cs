using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class NegativeGameEventService: INegativeGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public NegativeGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
            
        }
        
        public async Task<ServiceResult<NegativeGameEventResult>> GetRandomNegativeGameEvent(string authenticatedUserId)
        {
            var gameEvents = await FindAsync(authenticatedUserId);
            var randomEvent = GameEventHelper.GetRandomNegativeGameEvent(gameEvents.Data);
            return new ServiceResult<NegativeGameEventResult>(randomEvent);
        }

        public async Task<ServiceResult<IList<NegativeGameEventResult>>> FindAsync(string authenticatedUserId)
        {
            var negativeGameEvents = await _database.NegativeGameEvents
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<NegativeGameEventResult>>(negativeGameEvents);
        }

        public async Task<ServiceResult<NegativeGameEventResult>> EditAsync(int id, NegativeGameEventRequest request, string authenticatedUserId)
        {
            var original = await _database.NegativeGameEvents.FindAsync(id);
            if (original is null)
            {
                return new ServiceResult<NegativeGameEventResult>();
            }
            
            
            original.DefenseLoss = request.DefenseLoss;
            original.DefenseWithGearDescription = request.DefenseWithGearDescription;
            original.Description = request.Description;
            original.Name = request.Name;
            original.DefenseWithoutGearDescription = request.DefenseWithoutGearDescription;
            original.Probability = request.Probability;
            await _database.SaveChangesAsync();
            var negEventDb = (await _database.NegativeGameEvents
                .ProjectToResult()
                .ToListAsync())
                .SingleOrDefault(e => e.Id == request.Id);

            if (negEventDb is null)
            {
                return new ServiceResult<NegativeGameEventResult>();
            }
            return new ServiceResult<NegativeGameEventResult>(negEventDb);



        }
        public async Task<ServiceResult<NegativeGameEventResult>> CreateAsync(NegativeGameEventRequest request, string authenticatedUserId)
        {
            var gameEvent = new NegativeGameEvent
            {
                DefenseLoss = request.DefenseLoss,
                DefenseWithGearDescription = request.DefenseWithGearDescription,
                Description = request.Description,
                Name = request.Name,
                DefenseWithoutGearDescription = request.DefenseWithoutGearDescription,
                Probability = request.Probability
            };
            
            _database.NegativeGameEvents.Add(gameEvent);
            await _database.SaveChangesAsync();
            var negEventDb = (await _database.NegativeGameEvents
                    .ProjectToResult()
                    .ToListAsync())
                .SingleOrDefault(e => e.Name == request.Name);

            if (negEventDb is null)
            {
                return new ServiceResult<NegativeGameEventResult>();
            }
            return new ServiceResult<NegativeGameEventResult>(negEventDb);

        }
        public async Task<ServiceResult<bool>> DeleteAsync(int id, string authenticatedUserId)
        {

            var toRemoveObject =  _database.NegativeGameEvents.SingleOrDefault(e => e.Id == id);
            if (toRemoveObject is null)
            {
                return new ServiceResult<bool>(false);
            }
            _database.NegativeGameEvents.Remove(toRemoveObject);
            await _database.SaveChangesAsync();
            return new ServiceResult<bool>(true);

        }
    }
}
