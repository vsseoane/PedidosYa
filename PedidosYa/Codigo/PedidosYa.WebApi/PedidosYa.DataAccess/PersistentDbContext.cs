using PedidosYa.Models;
using System.Data.Entity;

namespace PedidosYa.DataAccess
{
    public class PersistentDbContext : DbContext
    {
        public PersistentDbContext() : base("name=PedidosYa") { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
