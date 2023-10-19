using lab3;
using lab3.Commands;
using lab3.Models;
using lab3.Workflow;

internal class Program
{
    static void Main(string[] args)
    {
        var products = ReadListOfProducts().ToArray();
        PayShoppingCartCommands command = new(products);
        PayShoppingCartWorkflow workflow = new();
        var result = workflow.Execute(command, (productName) => true);

        result.Match(
            whenShoppingCartPayFailedEvent: @event =>
            {
                Console.WriteLine($"Pay failed: {@event.Reason}");
                return @event;
            },
            whenShoppingCartPaySuccededEvent: @event =>
            {
                Console.WriteLine($"Pay succeded: {@event.Total}");
                return @event;
            }
            );

        Console.WriteLine("Done");
    }

    private static List<UnvalidatedShoppingCartProduct> ReadListOfProducts()
    {
        List<UnvalidatedShoppingCartProduct> listOfProducts = new();
        do
        {
            var productName = ReadValue("Product name: ");
            if (string.IsNullOrEmpty(productName))
            {
                break;
            }

            var price = ReadValue("Price: ");
            if (string.IsNullOrEmpty(price))
            {
                break;
            }

            listOfProducts.Add(new(productName, price));
        } while (true);
        return listOfProducts;
    }

    private static string? ReadValue(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}