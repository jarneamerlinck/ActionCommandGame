using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class ItemsController : ApiBaseController
    {
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;

        public ItemsController(IItemService itemService, 
            IPlayerItemService playerItemService)
        {
            _itemService = itemService;
            _playerItemService = playerItemService;
        }

        [HttpGet("items")]
        public async Task<IActionResult> Find()
        {
            var result = await _itemService.FindAsync(User.GetId());
            return Ok(result);
        }
        [HttpPost("items/{id}")]
        public async Task<IActionResult> Edit([FromBody] ItemRequest request)
        {
            var result = await _itemService.EditAsync(request.Id, request, User.GetId());
            return Ok(result);
        }
        [HttpPost("items/")]
        public async Task<IActionResult> Create([FromBody] ItemRequest request)
        {
            var result = await _itemService.CreateAsync(request, User.GetId());
            return Ok(result);
        }
        [HttpPost("items/Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //var playerItemList = await getAllPlayerItems(id);
            //foreach (var payerItem in playerItemList)
            //{
            //    var result = await _playerItemService.DeleteAsync(payerItem.Id, User.GetId());
            //    if (!result.IsSuccess)
            //    {
            //        return Ok(new ServiceResult<bool>(false));
            //    }
            //}
            var endResult = await _itemService.DeleteAsync(id, User.GetId());
            return Ok(endResult);
        }

        //private async Task<IList<PlayerItemResult>> getAllPlayerItems(int itemId)
        //{
        //    var preResult = await _playerItemService.FindAsync(new PlayerItemFilter
        //    {
        //        ItemId = itemId
        //    }, User.GetId());
        //    if (!preResult.IsSuccess)
        //    {
        //        return new List<PlayerItemResult>();
        //    }

        //    return preResult.Data;
        //}
    }
}
