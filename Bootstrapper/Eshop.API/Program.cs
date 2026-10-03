using Carter;
using Catalog.Contracts.Products.Feature.GetProductByIdQuery;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Shared.Data.Interceptors;
using Shared.Exceptions.Handler;
using Shared.Extentions;
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
var CatalogContractsAssembly = typeof(GetProductByIdQuery).Assembly;

builder.Services.AddCarterWithAssmblies(CatalogAssembly, BasketAssembly, OrderingAssembly);
builder.Services.AddMediatRWithAssmblies(CatalogAssembly, BasketAssembly, OrderingAssembly, CatalogContractsAssembly);

builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

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

await app.UseCatalogModuleAsync();
await app.UseBasketModuleAsync();
await app.UseOrderingModuleAsync();


app.Run();
