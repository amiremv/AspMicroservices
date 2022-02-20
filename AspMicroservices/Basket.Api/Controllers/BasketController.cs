using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
    }

    [HttpGet]
    public async Task<ActionResult<ShoppingCart>> Get(string userName)
    {
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        var basket = await _basketRepository.Get(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> Update([FromBody] ShoppingCart cart)
    {
        var basket = await _basketRepository.Update(cart.UserName, cart);
        return basket;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string userName)
    {
        await _basketRepository.Delete(userName);
        return Ok();
    }
}