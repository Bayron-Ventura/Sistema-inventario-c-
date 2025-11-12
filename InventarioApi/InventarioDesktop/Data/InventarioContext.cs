using Microsoft.EntityFrameworkCore;
using InventarioDesktop.Models;

namespace InventarioDesktop.Data
{
    public class InventarioContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public InventarioContext(DbContextOptions<InventarioContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseMySql("server=localhost;database=inventario_db;user=root;password=", 
                    new MySqlServerVersion(new Version(8, 0, 21)));
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
    }
}