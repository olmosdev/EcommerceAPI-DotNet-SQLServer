using System;

namespace CsBases.Fundamentals._05_Adapter_Pattern;

public class ProductAdapter
{
    public static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Name = product.Name ?? "Unnamed",
            Price = product.Price,
            Code = $"P-{product.Id}-{product.UniqueCode.ToString().Substring(0, 8)}"
        };
    }
}
