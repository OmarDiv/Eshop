using Shared.Extentions;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddCarterWithAssmblies(typeof(CatalogModule).Assembly, typeof(BasketModule).Assembly, typeof(OrderingModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();

await app.UseCatalogModuleAsync();
await app.UseBasketModuleAsync();
await app.UseOrderingModuleAsync();


app.Run();
