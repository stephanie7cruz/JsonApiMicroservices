using InventoryService.Interfaces;
using InventoryService.Services;
using InventoryService.Middleware;
using InventoryService.Clients;
using Microsoft.EntityFrameworkCore;
using InventoryService.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InventoryService",
        Version = "v1"
    });

    // ✅ Definición de la API Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key necesaria para acceder. Usa el header: X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    // ✅ Requerimiento para aplicarlo a todos los endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHealthChecks();

// Configura EF Core con tu cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias personalizada
builder.Services.AddSingleton<IInventoryService, InventoryService.Services.InventoryService>();

// Cliente HTTP para comunicarse con ProductService
builder.Services.AddHttpClient("ProductService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000"); // Cambia si usas otro puerto
    client.DefaultRequestHeaders.Add("X-API-KEY", "super-secreta-productos");
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5071);  
});

builder.Services.AddSingleton<ProductClient>();

var app = builder.Build();

// Middleware y configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware personalizado para validar la API Key
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

// Ejecuta el seeder
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

// Endpoints
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
