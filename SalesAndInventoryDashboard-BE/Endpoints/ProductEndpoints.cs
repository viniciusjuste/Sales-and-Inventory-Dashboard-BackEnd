using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Endpoints
{
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
            }).RequireAuthorization();


            app.MapGet("/products", async (AppDbContext context) =>
            {
                try
                {
                    var products = await context.Products.Where(p => p.IsActive == true).ToListAsync();
                    return Results.Ok(products);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting products: {ex.Message}");
                    return Results.Problem("An error occurred while trying to get the products. Please try again later.");
                }
            }).RequireAuthorization();

            app.MapGet("/products/name", async (string name, AppDbContext context) =>
            {
                try
                {
                    var products = await context.Products
                        .Where(p => p.IsActive == true &&
                                    p.Name != null &&
                                    p.Name.ToLower() == name.ToLower())
                        .ToListAsync();

                    return Results.Ok(products);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting products by name: {ex.Message}");
                    return Results.Problem("An error occurred while trying to get the products by name. Please try again later.");
                }
         }).RequireAuthorization();

            app.MapPatch("/products/{id}", async (int id, Product product, AppDbContext context) =>
            {
                try
                {
                    var existingProduct = await context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return Results.NotFound();
                    }

                    existingProduct.UpdatedDate = DateTime.UtcNow;

                    if (!string.IsNullOrWhiteSpace(product.Name) && product.Name != "string")
                        existingProduct.Name = product.Name;

                    if (product.Price.HasValue && product.Price.Value > 0)
                        existingProduct.Price = product.Price.Value;

                    if (!string.IsNullOrWhiteSpace(product.Description) && product.Description != "string")
                        existingProduct.Description = product.Description;

                    if (product.StockQuantity.HasValue && product.StockQuantity.Value > 0)
                        existingProduct.StockQuantity = product.StockQuantity.Value;

                    if (!string.IsNullOrWhiteSpace(product.Category) && product.Category != "string")
                        existingProduct.Category = product.Category;

                    if (product.IsActive.HasValue)
                        existingProduct.IsActive = product.IsActive.Value;

                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                    return Results.Problem("An error occurred while trying to update the product. Please try again later.");
                }
            }).RequireAuthorization();

            app.MapDelete("/products/{id}", async (int id, AppDbContext context) =>
            {
                try
                {
                    var product = await context.Products.FindAsync(id);
                    if (product == null)
                    {
                        return Results.NotFound();
                    }

                    product.IsActive = false;
                    product.UpdatedDate = DateTime.UtcNow;
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting product: {ex.Message}");
                    return Results.Problem("An error occurred while trying to delete the product. Please try again later.");
                }
            }).RequireAuthorization();
        }
    }
}