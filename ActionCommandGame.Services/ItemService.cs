using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class ItemService: IItemService
    {
        private readonly ActionCommandGameDbContext _database;

        public ItemService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<ItemResult>> GetAsync(int id, string authenticatedUserId)
        {
            var item = await _database.Items
                .ProjectToResult()
                .SingleOrDefaultAsync(i => i.Id == id);

            return new ServiceResult<ItemResult>(item);
        }

        public async Task<ServiceResult<IList<ItemResult>>> FindAsync(string authenticatedUserId)
        {
            var items = await _database.Items
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<ItemResult>>(items);
        }
        public async Task<ServiceResult<ItemResult>> EditAsync(int id, ItemRequest request, string authenticatedUserId)
        {
            var original = await _database.Items.FindAsync(id);
            if (original is null)
            {
                return new ServiceResult<ItemResult>();
            }


            original.Name = request.Name;
            original.Description = request.Description;
            original.ActionCooldownSeconds = request.ActionCooldownSeconds;
            original.Price = request.Price;
            original.Attack = request.Attack;
            original.Defense = request.Defense;
            original.Fuel = request.Fuel;

            

            await _database.SaveChangesAsync();
            var itemDb = (await _database.Items
                .ProjectToResult()
                .ToListAsync())
                .SingleOrDefault(e => e.Id == request.Id);

            if (itemDb is null)
            {
                return new ServiceResult<ItemResult>();
            }
            return new ServiceResult<ItemResult>(itemDb);



        }
        public async Task<ServiceResult<ItemResult>> CreateAsync(ItemRequest request, string authenticatedUserId)
        {
            var item = new Item()
            {
                Name = request.Name,
                Description = request.Description,
                ActionCooldownSeconds = request.ActionCooldownSeconds,
                Price = request.Price,
                Attack = request.Attack,
                Defense = request.Defense,
                Fuel = request.Fuel
            };

            _database.Items.Add(item);
            await _database.SaveChangesAsync();
            var itemDb = (await _database.Items
                    .ProjectToResult()
                    .ToListAsync())
                .SingleOrDefault(e => e.Name == request.Name);

            if (itemDb is null)
            {
                return new ServiceResult<ItemResult>();
            }
            return new ServiceResult<ItemResult>(itemDb);

        }
        public async Task<ServiceResult<bool>> DeleteAsync(int id, string authenticatedUserId)
        {

            var toRemoveObject = _database.Items.SingleOrDefault(i => i.Id == id);
            if (toRemoveObject is null)
            {
                return new ServiceResult<bool>(false);
            }
            var toRemovePlayerItems = _database.PlayerItems.Where(pi => pi.ItemId == id);

            if (toRemovePlayerItems is not null)
            {
                foreach (var toRemovePlayerItem in toRemovePlayerItems)
                {
                    _database.PlayerItems.Remove(toRemovePlayerItem);
                    await _database.SaveChangesAsync();
                }
            }
            _database.Items.Remove(toRemoveObject);
            await _database.SaveChangesAsync();
            return new ServiceResult<bool>(true);

        }
    }
}
