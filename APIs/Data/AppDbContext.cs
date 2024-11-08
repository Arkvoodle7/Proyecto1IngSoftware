using Microsoft.EntityFrameworkCore;
using APIs.Models;

namespace APIs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Especificar el nombre exacto de la tabla en Oracle
            modelBuilder.Entity<Producto>()
                .ToTable("PRODUCTOS");

            // Configurar la clave primaria
            modelBuilder.Entity<Producto>()
                .HasKey(p => p.Codigo);

            // Mapear las propiedades al nombre exacto de las columnas en Oracle
            modelBuilder.Entity<Producto>()
                .Property(p => p.Codigo)
                .HasColumnName("CODIGO");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Descripcion)
                .HasColumnName("DESCRIPCION");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnName("PRECIO")
                .HasPrecision(10, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.UnidadesDisponibles)
                .HasColumnName("UNIDADES_DISPONIBLES");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Disparador)
                .HasColumnName("DISPARADOR");

            base.OnModelCreating(modelBuilder);
        }
    }
}
