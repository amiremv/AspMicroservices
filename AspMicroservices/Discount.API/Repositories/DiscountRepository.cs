using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configurations;

    public DiscountRepository(IConfiguration configurations)
    {
        _configurations = configurations;
    }

    public async Task<Coupon> Get(string product_name)
    {
        await using var connection =
            new NpgsqlConnection(_configurations.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "SELECT * FROM Coupon WHERE Coupon.ProductName = @product_name", new
            {
                ProductName = product_name
            });
        return coupon ?? new Coupon {ProductName = "No discount", Description = "No discount", Amount = 0};
    }

    public async Task<bool> Create(Coupon coupon)
    {
        await using var connection =
            new NpgsqlConnection(_configurations.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected =
            await connection.ExecuteAsync(
                "INSERT INTO Coupon(ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)",
                new {coupon.ProductName, coupon.Description, coupon.Amount});
        return affected != 0;
    }

    public async Task<bool> Update(Coupon coupon)
    {
        await using var connection =
            new NpgsqlConnection(_configurations.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected =
            await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE Id = @Id)",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                    coupon.Id
                });
        return affected != 0;
    }

    public async Task<bool> Delete(string product_name)
    {
        await using var connection =
            new NpgsqlConnection(_configurations.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected =
            await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new
                {
                    ProductName = product_name
                });
        return affected != 0;
    }
}