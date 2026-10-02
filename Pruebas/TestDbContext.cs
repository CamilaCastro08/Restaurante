using Microsoft.EntityFrameworkCore;
using Libreria_restaurante.Entidades;

namespace Libreria_restaurante.Implementaciones
{
    public class TestDbContext : DbContext
    {
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Cocineros>? Cocineros { get; set; }
        public DbSet<Detalles_Pedidos>? Detalles_Pedidos { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Facturas>? Facturas { get; set; }
        public DbSet<Gerentes>? Gerentes { get; set; }
        public DbSet<Ingredientes>? Ingredientes { get; set; }
        public DbSet<Inventarios>? Inventarios { get; set; }
        public DbSet<Menus>? Menus { get; set; }
        public DbSet<Mesas>? Mesas { get; set; }
        public DbSet<Meseros>? Meseros { get; set; }
        public DbSet<MetodosPagos>? MetodosPagos { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<Pedidos>? Pedidos { get; set; }
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Recetas>? Recetas { get; set; }
        public DbSet<Reservas>? Reservas { get; set; }
        public DbSet<Turnos>? Turnos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Testdb_InMemory");
        }
    }
}