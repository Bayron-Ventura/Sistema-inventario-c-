using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace InventarioDesktop.Data
{
    public class InventarioContextFactory : IDesignTimeDbContextFactory<InventarioContext>
    {
        public InventarioContext CreateDbContext(string[] args)
        {
            // Cargar la configuración desde appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "appsettings.json"), optional: false)


                .Build();

            string connectionString = config.GetConnectionString("InventarioDB")
                ?? throw new InvalidOperationException("Connection string 'InventarioDB' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<InventarioContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));

            return new InventarioContext(optionsBuilder.Options);
        }
    }
}
