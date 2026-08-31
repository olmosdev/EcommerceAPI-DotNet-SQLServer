using CsBases.Fundamentals;
using CsBases.Fundamentals._05_Adapter_Pattern;
using CsBases.Fundamentals._06_Dependency_Injection;
using CsBases.Fundamentals._07_Asynchronous_Methods;

class Program
{
    public static async Task Main()
    {
        var laptop = new Product("Laptop", 1200);
        WriteLine(laptop.GetDescription());
        var support = new ServiceProduct("Technical Support", 300, 30);
        WriteLine(support.GetDescription());
        var product = new Product("Gamer Mouse", 300);
        var productDto = ProductAdapter.ToDto(product);
        WriteLine($"{productDto.Name} - {productDto.Price:C} - Code: {productDto.Code}");

        WriteLine();

        // Dependency injection
        ILabelService labelService = new LabelService();
        var manager = new ProductManager(labelService);
        var monitor = new Product("Monitor", 100);
        var installation = new ServiceProduct("Monitor installation", 20, 30);
        manager.PrintLabel(monitor);
        manager.PrintLabel(installation);

        WriteLine();

        // Asynchronous Methods
        var firstProduct = await new ProductRepository().GetProduct(1);
        firstProduct.Description = "This is a Description for the First Product";
        AttributeProcessor.ApplyUpperCase(firstProduct);
        WriteLine($"{firstProduct.Name} - {firstProduct.Price:C} - {firstProduct.Description}");
    }
}
