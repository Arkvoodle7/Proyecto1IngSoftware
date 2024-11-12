using Microsoft.EntityFrameworkCore;
using APIs.Models;

namespace APIs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<ProductoXFact> ProductosXFact { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("PRODUCTOS");
                entity.HasKey(p => p.Codigo);

                entity.Property(p => p.Codigo)
                     .HasColumnName("CODIGO")
                     .HasColumnType("NUMBER(19,0)");

                entity.Property(p => p.Descripcion).HasColumnName("DESCRIPCION");
                entity.Property(p => p.Precio)
                    .HasColumnName("PRECIO")
                    .HasColumnType("NUMBER(10,2)");
                entity.Property(p => p.UnidadesDisponibles)
                    .HasColumnName("UNIDADES_DISPONIBLES")
                    .HasColumnType("NUMBER(10,0)");
                entity.Property(p => p.Disparador)
                    .HasColumnName("DISPARADOR")
                    .HasColumnType("NUMBER(1)")
                    .HasConversion<int>(); // Indica cómo convertir entre bool y NUMBER
            });

            // Configuración de la entidad Factura
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("FACTURAS");
                entity.HasKey(f => f.NumFact);

                entity.Property(f => f.NumFact)
                    .HasColumnName("NUM_FACT")
                    .HasColumnType("NUMBER(19,0)")
                    .HasPrecision(19, 0)
                    .ValueGeneratedOnAdd();

                entity.Property(f => f.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("DATE");

                entity.Property(f => f.Total)
                    .HasColumnName("TOTAL")
                    .HasColumnType("NUMBER(10,2)");

                entity.Property(f => f.IdCliente)
                   .HasColumnName("ID_CLIENTE")
                   .HasColumnType("NUMBER(19,0)");
            });

            // Configuración de la entidad ProductoXFact
            modelBuilder.Entity<ProductoXFact>(entity =>
            {
                entity.ToTable("PRODUCTOSXFACT");
                entity.HasKey(px => new { px.Fact, px.Producto });

                entity.Property(px => px.Fact)
                    .HasColumnName("FACT")
                    .HasColumnType("NUMBER(19,0)");

                entity.Property(px => px.Producto)
                    .HasColumnName("PRODUCTO")
                    .HasColumnType("NUMBER(19,0)");
            });

            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIOS");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(19,0)");

                entity.Property(u => u.Nombre)
                    .HasColumnName("NOMBRE")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(u => u.Apellido1)
                    .HasColumnName("APELLIDO1")
                    .HasColumnType("VARCHAR2(30)");

                entity.Property(u => u.Apellido2)
                    .HasColumnName("APELLIDO2")
                    .HasColumnType("VARCHAR2(30)");

                entity.Property(u => u.Telefono)
                    .HasColumnName("TELEFONO")
                    .HasColumnType("NUMBER");

                entity.Property(u => u.Correo)
                    .HasColumnName("CORREO")
                    .HasColumnType("VARCHAR2(30)");

                entity.Property(u => u.Contrasena)
                    .HasColumnName("CONTRASENA")
                    .HasColumnType("VARCHAR2(30)");

                entity.Property(u => u.Rol)
                    .HasColumnName("ROL")
                    .HasColumnType("VARCHAR2(20)");
            });

        }
    }
}