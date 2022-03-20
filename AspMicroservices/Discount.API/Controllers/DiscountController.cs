using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    [HttpGet(nameof(GetDiscount))]
    public async Task<ActionResult<Coupon>> GetDiscount(string product_name)
    {
        if (product_name == null) throw new ArgumentNullException(nameof(product_name));
        var coupon = await _discountRepository.Get(product_name);
        return Ok(coupon);
    }

    [HttpPost(nameof(Create))]
    public async Task<ActionResult<Coupon>> Create([FromBody] Coupon coupon)
    {
        await _discountRepository.Create(coupon);
        return Ok(coupon);
    }

    [HttpPut(nameof(Update))]
    public async Task<ActionResult<Coupon>> Update([FromBody] Coupon coupon)
    {
        return Ok(await _discountRepository.Update(coupon));
    }

    [HttpDelete(nameof(Update))]
    public async Task<ActionResult<bool>> Update(string product_name)
    {
        var deletedDiscount = await _discountRepository.Delete(product_name);
        return Ok(deletedDiscount);
    }
}