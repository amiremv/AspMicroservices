using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
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
        foreach (var item in cart.CartItems)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }

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