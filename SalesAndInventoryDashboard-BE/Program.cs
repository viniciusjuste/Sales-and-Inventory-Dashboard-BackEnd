using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("SalesAndInventoryDashboard"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "SalesAndInventoryDashboard";
    config.Title = "SalesAndInventoryDashboard v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "SalesAndInventoryDashboard";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

SaleEndpoints.MapSaleEndpoints(app);
ProductEndPoints.MapProductEndpoints(app);
ReportEndpoints.MapReportEndpoints(app);

app.Run();
