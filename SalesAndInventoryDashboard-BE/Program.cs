using Microsoft.EntityFrameworkCore;
using SalesAndInventoryDashboard_BE.Data;
using SalesAndInventoryDashboard_BE.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados com SQL Server
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Tratamento de exceções detalhado em tempo de desenvolvimento
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuração do Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "SalesAndInventoryDashboard";
    config.Title = "SalesAndInventoryDashboard v1";
    config.Version = "v1";
});

var app = builder.Build();

// Habilita o Swagger se estiver em ambiente de desenvolvimento
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

// Usa CORS
app.UseCors("AllowAll");

// Mapeia os endpoints definidos em outras classes
SaleEndpoints.MapSaleEndpoints(app);
ProductEndPoints.MapProductEndpoints(app);
ReportEndpoints.MapReportEndpoints(app);
RegisterUser.MapRegisterUserEndpoint(app);
LoginUser.MapLoginUserEndpoint(app);

app.Run();
