namespace MiniECommerce.Orders.WebAPI.Dtos;

public sealed class Result<T>
{
    public Result()
    {
        
    }

    public Result(T _Data)
    {
        Data = _Data;
    }
    public T? Data { get; set; }
}
