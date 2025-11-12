using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using InventarioDesktop.Data;

namespace InventarioDesktop.Data
{
    public class InventarioContextFactory : IDesignTimeDbContextFactory<InventarioContext>
    {
        public InventarioContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<InventarioContext>();
            
            optionsBuilder.UseMySql("server=localhost;database=inventario_db;user=root;password=", 
                new MySqlServerVersion(new Version(8, 0, 21)));

            return new InventarioContext(optionsBuilder.Options);
        }
    }
}