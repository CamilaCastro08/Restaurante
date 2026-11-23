using Libreria_restaurante.Entidades;
using Libreria_restaurante.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Libreria_restaurante.Implementaciones
{
    public class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Categorias>? Categorias { get; set; }
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
        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Permisos>? Permisos { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<CategoriasAuditorias>? CategoriasAuditorias { get; set; }
        public DbSet<ClientesAuditorias>? ClientesAuditorias { get; set; }
        public DbSet<CocinerosAuditorias>? CocinerosAuditorias { get; set; }
        public DbSet<Detalles_PedidosAuditorias>? Detalles_PedidosAuditorias { get; set; }
        public DbSet<EmpleadosAuditorias>? EmpleadosAuditorias { get; set; }
        public DbSet<FacturasAuditorias>? FacturasAuditorias { get; set; }
        public DbSet<GerentesAuditorias>? GerentesAuditorias { get; set; }
        public DbSet<IngredientesAuditorias>? IngredientesAuditorias { get; set; }
        public DbSet<InventariosAuditorias>? InventariosAuditorias { get; set; }
        public DbSet<MenusAuditorias>? MenusAuditorias { get; set; }
        public DbSet<MesasAuditorias>? MesasAuditorias { get; set; }
        public DbSet<MeserosAuditorias>? MeserosAuditorias { get; set; }
        public DbSet<MetodosPagosAuditorias>? MetodosPagosAuditorias { get; set; }
        public DbSet<PagosAuditorias>? PagosAuditorias { get; set; }
        public DbSet<PedidosAuditorias>? PedidosAuditorias { get; set; }
        public DbSet<ProductosAuditorias>? ProductosAuditorias { get; set; }
        public DbSet<ProveedoresAuditorias>? ProveedoresAuditorias { get; set; }
        public DbSet<RecetasAuditorias>? RecetasAuditorias { get; set; }
        public DbSet<ReservasAuditorias>? ReservasAuditorias { get; set; }
        public DbSet<TurnosAuditorias>? TurnosAuditorias { get; set; }
        public DbSet<UsuariosAuditorias>? UsuariosAuditorias { get; set; }
        public DbSet<PermisosAuditorias>? PermisosAuditorias { get; set; }
        public DbSet<Roles>? RolesAuditorias { get; set; }
    }
}
