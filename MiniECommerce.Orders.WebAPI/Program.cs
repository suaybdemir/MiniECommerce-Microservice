using MiniECommerce.Orders.WebAPI.Context;
using MiniECommerce.Orders.WebAPI.Dtos;
using MiniECommerce.Orders.WebAPI.Models;
using MiniECommerce.Orders.WebAPI.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<MongoDbSettings>(builder
    .Configuration.GetSection("MongoDbSettings")
    );

builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", async(MongoDbContext context,IConfiguration configuration,CancellationToken cancellationToken) =>
{
    var items = context.GetCollection<Order>("Orders");

    var orders = await items.Find(items=>true).ToListAsync(cancellationToken);

    if(orders is null)
    {
        return Results.Ok(new Result<string>());
    }

    List<OrderDto> orderDtos = new();

    Result<List<ProductDto>>? products = new();

    HttpClient httpClient = new HttpClient();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await httpClient.GetAsync(productsEndpoint);

    if(message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);
    }

    if (products?.Data == null || !products.Data.Any())
    {
        throw new Exception("Product list is empty or null.");
    }

    foreach (var order in orders)
    {
        var product = products.Data.FirstOrDefault(p => p.Id == order.ProductId);
        if (product == null)
        {
            Console.WriteLine($"No product found for ProductId: {order.ProductId}");
            continue; // Hata vermek yerine sipariþi atlýyoruz.
        }

        OrderDto orderDto = new()
        {
            Id = order.Id,
            CreateAt = order.CreateAt,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Price = order.Price,
            ProductName = product.Name
        };

        orderDtos.Add(orderDto);
    }


    return Results.Ok(new Result<List<OrderDto>>(orderDtos));
});


app.MapPost("/create", async(MongoDbContext context,List<CreateOrderDto> request,CancellationToken cancellationToken) =>
{
    var items = context.GetCollection<Order>("Orders");

    List<Order> orders = new();

    foreach (var item in request)
    {
        Order order = new()
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Price = item.Price,
            CreateAt = DateTime.Now
        };


        orders.Add(order);
    }

    await items.InsertManyAsync(orders,cancellationToken:cancellationToken);


    return Results.Ok(new Result<string>("Order has successfully created!"));
});

app.Run();
