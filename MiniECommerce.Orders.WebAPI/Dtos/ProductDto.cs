namespace MiniECommerce.Orders.WebAPI.Dtos;

public sealed record ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
