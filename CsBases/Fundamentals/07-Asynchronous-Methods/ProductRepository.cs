using CsBases.Fundamentals;

namespace CsBases.Fundamentals._07_Asynchronous_Methods;

public class ProductRepository
{
    public async Task<Product> GetProduct(int id)
    {
        WriteLine("Searching product...");
        await Task.Delay(2000);
        return new Product("Simulated product", 500);
    }
}
