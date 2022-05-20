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
    public class PositiveGameEventService: IPositiveGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public PositiveGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PositiveGameEventResult>> GetRandomPositiveGameEvent(bool hasAttackItem, string authenticatedUserId)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = await query
                .ProjectToResult()
                .ToListAsync();

            var randomEvent = GameEventHelper.GetRandomPositiveGameEvent(gameEvents);

            return new ServiceResult<PositiveGameEventResult>(randomEvent);
        }
        public async Task<ServiceResult<IList<PositiveGameEventResult>>> FindAsync(string authenticatedUserId)
        {
            var positiveGameEvents = await _database.PositiveGameEvents
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<PositiveGameEventResult>>(positiveGameEvents);
        }

        public async Task<ServiceResult<PositiveGameEventResult>> EditAsync(int id, PositiveGameEventRequest request, string authenticatedUserId)
        {
            var original = await _database.PositiveGameEvents.FindAsync(id);
            if (original is null)
            {
                return new ServiceResult<PositiveGameEventResult>();
            }


            original.Money = request.Money;
            original.Experience = request.Experience;
            original.Description = request.Description;
            original.Name = request.Name;
            original.Probability = request.Probability;

            await _database.SaveChangesAsync();
            var posEventDb = (await _database.PositiveGameEvents
                .ProjectToResult()
                .ToListAsync())
                .SingleOrDefault(e => e.Id == request.Id);

            if (posEventDb is null)
            {
                return new ServiceResult<PositiveGameEventResult>();
            }
            return new ServiceResult<PositiveGameEventResult>(posEventDb);



        }
        public async Task<ServiceResult<PositiveGameEventResult>> CreateAsync(PositiveGameEventRequest request, string authenticatedUserId)
        {
            var gameEvent = new PositiveGameEvent
            {
                Money = request.Money,
                Experience = request.Experience,
                Description = request.Description,
                Name = request.Name,
                Probability = request.Probability
            };

            _database.PositiveGameEvents.Add(gameEvent);
            await _database.SaveChangesAsync();
            var posEventDb = (await _database.PositiveGameEvents
                    .ProjectToResult()
                    .ToListAsync())
                .SingleOrDefault(e => e.Name == request.Name);

            if (posEventDb is null)
            {
                return new ServiceResult<PositiveGameEventResult>();
            }
            return new ServiceResult<PositiveGameEventResult>(posEventDb);

        }
        public async Task<ServiceResult<bool>> DeleteAsync(int id, string authenticatedUserId)
        {

            var toRemoveObject = _database.PositiveGameEvents.SingleOrDefault(e => e.Id == id);
            if (toRemoveObject is null)
            {
                return new ServiceResult<bool>(false);
            }
            _database.PositiveGameEvents.Remove(toRemoveObject);
            await _database.SaveChangesAsync();
            return new ServiceResult<bool>(true);

        }
    }
}
