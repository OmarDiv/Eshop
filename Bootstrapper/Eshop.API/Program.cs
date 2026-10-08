using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Data.Interceptors;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(cfg =>
{
    cfg.Configuration = builder.Configuration.GetConnectionString("Redis");
});
var CatalogAssembly = typeof(CatalogModule).Assembly;
var BasketAssembly = typeof(BasketModule).Assembly;
var OrderingAssembly = typeof(OrderingModule).Assembly;

builder.Services.AddCarterWithAssmblies(CatalogAssembly, BasketAssembly, OrderingAssembly);
builder.Services.AddMediatRWithAssmblies(CatalogAssembly, BasketAssembly, OrderingAssembly);
builder.Services.AddMassTransitWithAssmblies(builder.Configuration, CatalogAssembly, BasketAssembly, OrderingAssembly);
builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

await app.UseCatalogModuleAsync();
await app.UseBasketModuleAsync();
await app.UseOrderingModuleAsync();


app.Run();
