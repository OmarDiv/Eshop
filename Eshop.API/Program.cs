using Carter;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extentions;
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddCarterWithAssmblies(
    typeof(CatalogModule).Assembly
   , typeof(BasketModule).Assembly
   , typeof(OrderingModule).Assembly);

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
