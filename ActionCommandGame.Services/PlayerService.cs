﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly ActionCommandGameDbContext _database;

        public PlayerService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PlayerResult>> GetAsync(int id, string authenticatedUserId)
        {
            var player = await _database.Players
                .ProjectToResult()
                .SingleOrDefaultAsync(p => p.Id == id);

            return new ServiceResult<PlayerResult>(player);
        }

        public async Task<ServiceResult<IList<PlayerResult>>> FindAsync(PlayerFilter filter, string authenticatedUserId)
        {
            
            var playersTemp = _database.Players
                .ApplyFilter(filter, authenticatedUserId)
                .ProjectToResult()
                ;

            var players = await playersTemp.ToListAsync();

            return new ServiceResult<IList<PlayerResult>>(players);
        }

        public async Task<ServiceResult<CreatePlayerResult>> CreatePlayer(CreatePlayerRequest playerRequest, string authenticatedUserId)
        {
            var player = new Player
            {
                Name = playerRequest.Name,
                ImageLocation = playerRequest.ImageLocation,
                UserId = authenticatedUserId,
                Money = 100

            };

            await _database.Players.AddAsync(player);
            await _database.SaveChangesAsync();
            var result = new CreatePlayerResult
            {
                Name = playerRequest.Name
            };
            return new ServiceResult<CreatePlayerResult>(result);

        }
    }
}
