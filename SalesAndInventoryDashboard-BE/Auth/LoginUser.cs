using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Endpoints
{
    public static class LoginUser
    {
        public static void MapLoginUserEndpoint(WebApplication app)
        {
            app.MapPost("/login", async (LoginDTO userLogin, AppDbContext context) =>
            {
                try
                {
                    var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
                    if (userInDb == null)
                    {
                        return Results.NotFound("User not found.");
                    }

                    var passwordHasher = new PasswordHasher<User>();

                    if (string.IsNullOrEmpty(userInDb.Password))
                    {
                        return Results.BadRequest("Invalid email or password.");
                    }

                    var result = passwordHasher.VerifyHashedPassword(userInDb, userInDb.Password!, userLogin.Password!);

                    if (result == PasswordVerificationResult.Success)
                    {
                        var userResponse = new
                        {
                            userInDb.Id,
                            userInDb.Name,
                            userInDb.Email
                        };

                        return Results.Ok(userResponse);
                    }
                    else
                    {
                        return Results.BadRequest("Invalid email or password.");
                    }
                }
                catch (Exception ex)
                {
                    return Results.Problem("An unexpected error occurred while logging in the user: " + ex.Message);
                }
            });
        }
    }
}