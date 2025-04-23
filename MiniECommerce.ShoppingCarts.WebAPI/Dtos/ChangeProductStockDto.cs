namespace MiniECommerce.ShoppingCarts.WebAPI.Dtos;

public sealed record ChangeProductStockDto(
Guid ProductId,
int Quantity);