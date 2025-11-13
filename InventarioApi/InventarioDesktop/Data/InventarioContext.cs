using Microsoft.EntityFrameworkCore;
using InventarioDesktop.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace InventarioDesktop.Data
{
    public class InventarioContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "appsettings.json"), optional: false)

                    .Build();

                string connectionString = config.GetConnectionString("InventarioDB") ?? throw new InvalidOperationException("Connection string 'InventarioDB' not found.");

                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("productos");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
                entity.Property(e => e.Precio).HasColumnName("precio").HasDefaultValue(0.00m).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Stock).HasColumnName("stock").HasDefaultValue(0);
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categorias");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
            });
        }
        
        public InventarioContext(DbContextOptions<InventarioContext> options)
    : base(options)
{
}

    }
}