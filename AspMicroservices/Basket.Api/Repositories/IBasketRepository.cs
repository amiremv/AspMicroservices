using Basket.Api.Entities;

namespace Basket.Api.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart?> Get(string userName);
    Task<ShoppingCart> Update(string userName, ShoppingCart basket);
    Task Delete(string userName);
}