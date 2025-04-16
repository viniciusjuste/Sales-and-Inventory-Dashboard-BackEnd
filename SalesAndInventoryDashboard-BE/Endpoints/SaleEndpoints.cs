using Microsoft.EntityFrameworkCore;
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

                    // Guarda os itens originais enviados na requisição
                    var originalItems = sale.Items.ToList();

                    // Inicializa a lista para evitar conflito de rastreamento
                    sale.Items = new List<SaleItem>();

                    // Salva a venda primeiro (sem itens)
                    context.Sales.Add(sale);
                    await context.SaveChangesAsync(); // Gera o sale.Id

                    // Agora adiciona os SaleItems corretamente
                    foreach (var item in originalItems)
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

                        product.StockQuantity -= item.Quantity;

                        var saleItem = new SaleItem
                        {
                            SaleId = sale.Id,
                            ProductId = item.ProductId,
                            ProductName = product.Name ?? "",
                            Quantity = item.Quantity,
                            UnitPrice = product.Price ?? 0
                        };

                        total += saleItem.Quantity * saleItem.UnitPrice;

                        context.SaleItems.Add(saleItem);
                    }

                    // Atualiza o total e salva tudo
                    sale.Total = total;
                    await context.SaveChangesAsync();

                    return Results.Created($"/sales/{sale.Id}", sale);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving sale: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Results.Problem("An error occurred while trying to save the sale. Please try again later.");
                }
            });

            app.MapGet("/sales", async (AppDbContext context) =>
            {
                try
                {
                    // Busca todas as vendas e inclui os itens relacionados
                    var sales = await context.Sales.Include(s => s.Items).ToListAsync();
                    if (sales.Count == 0)
                    {
                        return Results.NotFound();
                    }

                    // Retorna as vendas encontradas, com os itens relacionados
                    return Results.Ok(sales);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting sale: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Results.Problem("An error occurred while trying to get the sale. Please try again later.");
                }
            });

            app.MapGet("/sales/{id}", async (int id, AppDbContext context) =>
            {
                try
                {
                    // Busca a venda pelo id e inclui os itens relacionados
                    var sale = await context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);

                    if (sale == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(sale);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting sale: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return Results.Problem("An error occurred while trying to get the sale. Please try again later.");
                }
            });
        }
    }
}
