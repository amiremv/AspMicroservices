using Microsoft.AspNetCore.Http.Features;

namespace Basket.Api.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();

    private ShoppingCart()
    {
    }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            CartItems.ForEach(item => { totalPrice += item.Price * item.Quantity; });
            return totalPrice;
        }
    }
}