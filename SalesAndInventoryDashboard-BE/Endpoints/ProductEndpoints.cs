using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Endpoints;

public class ProductEndPoints
{
    public static void MapProductEndpoints(WebApplication app)
    {

        app.MapPost("/products", async (Product product, AppDbContext context) =>

        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return Results.BadRequest("Produto inválido. Verifique o nome e o preço.");
            }

            try
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Results.Created($"/products/{product.Id}", product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving product: {ex.Message}");
                return Results.Problem("An error occurred while trying to save the product. Please try again later.");
            }
        });

        app.MapGet("/products", async (AppDbContext context) =>
        {
            try
            {
                var products = await context.Products.FindAsync();
                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products: {ex.Message}");
                return Results.Problem("An error occurred while trying to get the products. Please try again later.");
            }
        });

        
    }
}