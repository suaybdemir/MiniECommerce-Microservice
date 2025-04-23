using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiniECommerce.ShoppingCarts.WebAPI.Context;
using MiniECommerce.ShoppingCarts.WebAPI.Dtos;
using MiniECommerce.ShoppingCarts.WebAPI.Models;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", async (ApplicationDbContext context, IConfiguration configuration, CancellationToken cancellationToken) =>
{
    List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.ToListAsync(cancellationToken);

    HttpClient client = new HttpClient();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await client.GetAsync(productsEndpoint);

    Result<List<ProductDto>>? products = new();

    if (message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);
    }

    List<ShoppingCartDto> response = shoppingCarts.Select(s => new ShoppingCartDto()
    {
        Id = s.Id,
        ProductId = s.ProductId,
        Quantity = s.Quantity,
        ProductName = products!.Data!.First(p => p.Id == s.ProductId).Name,
        ProductPrice = products!.Data!.First(p => p.Id == s.ProductId).Price

    }).ToList();

    return new Result<List<ShoppingCartDto>>(response);
});

app.MapPost("create", async (CreateShoppingCartDto request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    var existingCartItem = await context.ShoppingCarts.FirstOrDefaultAsync(sc=>sc.ProductId== request.ProductId,cancellationToken);

    if(existingCartItem != null)
    {
        existingCartItem.Quantity += request.Quantity;
        context.Update(existingCartItem);
    }
    else
    {
        ShoppingCart shoppingCart = new()
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        context.Add(shoppingCart);
    }
    
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(new Result<string>("Product has successfully added to shopping cart!"));
});

app.MapGet("/createOrder", async (ApplicationDbContext context,IConfiguration configuration,  CancellationToken cancellationToken) =>
{
    List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.ToListAsync(cancellationToken);
    HttpClient client = new HttpClient();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await client.GetAsync(productsEndpoint);

    Result<List<ProductDto>>? products = new();

    if (message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);
    }

    List<CreateOrderDto> response = shoppingCarts.Select(s => new CreateOrderDto {
        ProductId = s.ProductId,
        Quantity = s.Quantity,
        Price = products!.Data!.First(p => p.Id == s.ProductId).Price,
    }).ToList();

    string ordersEndpoint = $"http://{configuration.GetSection("HttpRequest:Orders").Value}/create";

    string stringJson = JsonSerializer.Serialize(response);
    var content = new StringContent(stringJson, Encoding.UTF8, "application/json");

    var orderMessage = await client.PostAsync(ordersEndpoint,content);

    if(orderMessage.IsSuccessStatusCode)
    {
        List<ChangeProductStockDto> changeProductStockDtos = shoppingCarts.Select(s => new
        ChangeProductStockDto(s.ProductId, s.Quantity)).ToList();

        productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/change-product-stock";

        string productsStringJson = JsonSerializer.Serialize(changeProductStockDtos);
        var productsContent = new StringContent(productsStringJson, Encoding.UTF8, "application/json");

        await client.PostAsync(productsEndpoint, productsContent);

        context.RemoveRange(shoppingCarts);
        await context.SaveChangesAsync(cancellationToken);
    }

    return Results.Ok(new Result<string>("Order creation successfully!"));
});

using(var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
