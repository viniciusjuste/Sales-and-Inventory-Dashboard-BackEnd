using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Endpoints
{
    public class SaleEndpoints
    {
        public static void MapSaleEndpoints(WebApplication app)
        {
            app.MapPost("/sales", async (Sale sale, AppDbContext context) =>
            {
                try
                {
                    decimal total = 0;
                    var saleItems = new List<SaleItem>(); // Lista temporária para armazenar os itens da venda

                    foreach (var item in sale.Items)
                    {
                        var product = await context.Products.FindAsync(item.ProductId);
                        if (product == null || !product.IsActive.GetValueOrDefault())
                        {
                            return Results.BadRequest($"Product ID {item.ProductId} not found or inactive.");
                        }

                        if (product.StockQuantity < item.Quantity)
                        {
                            return Results.BadRequest($"Insufficient stock for product {product.Name}.");
                        }

                        // Atualiza o estoque
                        product.StockQuantity -= item.Quantity;

                        // Cria o SaleItem
                        var saleItem = new SaleItem
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = product.Price ?? 0,
                        };

                        // Atualiza o total da venda
                        total += saleItem.Quantity * saleItem.UnitPrice;

                        // Adiciona o SaleItem na lista temporária
                        saleItems.Add(saleItem);
                    }

                    // Agora, adiciona os SaleItems à coleção e ao contexto
                    foreach (var saleItem in saleItems)
                    {
                        context.SaleItems.Add(saleItem);
                        sale.Items.Add(saleItem); // Aqui, você adiciona os itens à venda
                    }

                    // Atualiza o total da venda
                    sale.Total = total;

                    // Adiciona a venda no contexto
                    context.Sales.Add(sale);
                    Console.WriteLine($"Sale ID after Save: {sale.Id}");
                    Console.WriteLine("Antes de salvar as alterações no banco de dados.");

                    // Salva as alterações no banco de dados
                    await context.SaveChangesAsync();
                    Console.WriteLine("Depois de salvar as alterações no banco de dados.");

                    return Results.Created($"/sales/{sale.Id}", sale);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving sale: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Results.Problem("An error occurred while trying to save the sale. Please try again later.");
                }
            });
        }
    }
}
