using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesAndInventoryDashboard_BE.configuration;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Models;

namespace SalesAndInventoryDashboard_BE.Endpoints
{
    public static class LoginUser
    {
        public static void MapLoginUserEndpoint(WebApplication app)
        {
            app.MapPost("/login", async (LoginDTO userLogin, AppDbContext context, IOptions<JwtSettings> jwtOptions) =>
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
                    var jwtSettings = jwtOptions.Value;

                    // Criar os Claims
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userInDb.Id.ToString()), // ID do usuário
                        new Claim(JwtRegisteredClaimNames.Email, userInDb.Email!),       // Email
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único do token
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var expiration = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes);

                    var token = new JwtSecurityToken(
                        issuer: jwtSettings.Issuer,
                        audience: jwtSettings.Audience,
                        claims: claims,
                        expires: expiration,
                        signingCredentials: creds
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Results.Ok(new
                    {
                        Token = tokenString,
                        ExpiresAt = expiration
                    });
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