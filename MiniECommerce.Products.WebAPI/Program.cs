using Bogus;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Products.WebAPI.Context;
using MiniECommerce.Products.WebAPI.Dtos;
using MiniECommerce.Products.WebAPI.Model;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/seedData", (ApplicationDbContext context) =>
{
    for (int i = 0; i < 100; i++)
    {

        Faker faker = new();

        Product product = new()
        {
            Name = faker.Commerce.ProductName(),
            Price = Convert.ToDecimal(faker.Commerce.Price()),
            Stock = faker.Commerce.Random.Int(1, 100)
        };

        context.Add(product);
    }

    context.SaveChanges();

    return Results.Ok(Result<string>.Succeed("Seed data has ran and products creation has successsfully!"));
});

app.MapGet("/getall", async (ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    List<Product>? products = await context.Products.OrderBy(p => p.Name).ToListAsync(cancellationToken);

    Result<List<Product>> response = products;

    return response;
});

app.MapPost("/create", async (CreateProductDto request,ApplicationDbContext context, CancellationToken cancellationToken) =>
{

    bool isNameExists = await context.Products.AnyAsync(p => p.Name == request.Name, cancellationToken);

    if(isNameExists)
    {
        var response = Result<string>.Failure("This product name already exists!");
        return Results.BadRequest(response);
    }


    Product product = new()
    {
        Name = request.Name,
        Price = request.Price,
        Stock = request.Stock
    };

    context.Add(product);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Product record has successfully!"));

});

app.MapPost("/change-product-stock", async (List<ChangeProductDto> request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    foreach(var item in request)
    {
        Product? product = await context.Products!.FindAsync(item.ProductId, cancellationToken);
        if(product is not null)
        {
            product!.Stock -= item.Quantity;
        }
    }

    await context.SaveChangesAsync(cancellationToken);

    return Results.NoContent();
});

using (var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
