using System;

namespace CsBases.Fundamentals._06_Dependency_Injection;

public class ProductManager
{
    private readonly ILabelService _labelService;

    public ProductManager(ILabelService labelService)
    {
        _labelService = labelService;
    }

    public void PrintLabel(Product product)
    {
        var label = _labelService.GenerateLabel(product);
        WriteLine(label);
    }
}
