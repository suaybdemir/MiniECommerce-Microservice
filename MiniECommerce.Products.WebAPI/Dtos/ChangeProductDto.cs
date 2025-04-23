namespace MiniECommerce.Products.WebAPI.Dtos;

public sealed record ChangeProductDto(
    Guid ProductId,
    int Quantity);
