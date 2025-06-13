using SalesAndInventoryDashboard_BE.Models;
using SalesAndInventoryDashboard_BE.Data;
using Microsoft.AspNetCore.Identity;

namespace SalesAndInventoryDashboard_BE.Endpoints
{
    public static class RegisterUser
    {
        public static void MapRegisterUserEndpoint(WebApplication app)
        {
            app.MapPost("/register", async (User user, AppDbContext context) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(user.Name) ||
                    string.IsNullOrWhiteSpace(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Email))
                    {
                        return Results.BadRequest("Name, password, and email are required and cannot be empty or whitespace.");
                    }

                    if(!user.Email.Contains("@") || !user.Email.Contains("."))
                    {
                        return Results.BadRequest("Invalid email format.");
                    }

                    var passwordHasher = new PasswordHasher<User>();
                    user.Password = passwordHasher.HashPassword(user, user.Password);

                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                    
                    return Results.Created($"/users/{user.Id}", user);
                }
                catch (Exception ex)
                {
                return Results.Problem("An unexpected error occurred while registering the user." + ex.Message);
                }
            });
        }
    }
}