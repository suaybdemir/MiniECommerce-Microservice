using Microsoft.EntityFrameworkCore;
using MiniECommerce.Gateway.YARP.Models;

namespace MiniECommerce.Gateway.YARP.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options): base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}
