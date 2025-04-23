using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MiniECommerce.ShoppingCarts.WebAPI.Models;

namespace MiniECommerce.ShoppingCarts.WebAPI.Context;

public sealed class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options)
    {      
    }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
