using Discount.API.Entities;

namespace Discount.API.Repositories;

public interface IDiscountRepository
{
    Task<Coupon> Get(string product_name);
    Task<bool> Create(Coupon coupon);
    Task<bool> Update(Coupon coupon);
    Task<bool> Delete(string product_name);
}