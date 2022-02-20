using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
    }

    public async Task<ShoppingCart?> Get(string userName)
    {
        var basket = await _redisCache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> Update(string userName, ShoppingCart basket)
    {
        await _redisCache.SetStringAsync(userName, JsonConvert.SerializeObject(basket));
        return await Get(userName);
    }

    public async Task Delete(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }
}