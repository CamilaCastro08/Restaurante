using System;
using pruebas;

namespace Consl_restaurante
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando pruebas unitarias directas...");
            var testClasses = new (string Name, Action Action)[]
            {
                ("Categorias", () => new CategoriasPruebas().Execute()),
                ("Clientes", () => new ClientesPruebas().Execute()),
                ("Cocineros", () => new CocinerosPruebas().Execute()),
                ("Detalles_Pedidos", () => new Detalles_PedidosPruebas().Execute()),
                ("Empleados", () => new EmpleadosPruebas().Execute()),
                ("Facturas", () => new FacturasPruebas().Execute()),
                ("Gerentes", () => new GerentesPruebas().Execute()),
                ("Ingredientes", () => new IngredientesPruebas().Execute()),
                ("Inventarios", () => new InventariosPruebas().Execute()),
                ("Menus", () => new MenusPruebas().Execute()),
                ("Mesas", () => new MesasPruebas().Execute()),
                ("Meseros", () => new MeserosPruebas().Execute()),
                ("MetodosPagos", () => new MetodosPagosPruebas().Execute()),
                ("Pagos", () => new PagosPruebas().Execute()),
                ("Pedidos", () => new PedidosPruebas().Execute()),
                ("Permisos", () => new PermisosPruebas().Execute()),
                ("Productos", () => new ProductosPruebas().Execute()),
                ("Proveedores", () => new ProveedoresPruebas().Execute()),
                ("Recetas", () => new RecetasPruebas().Execute()),
                ("Reservas", () => new ReservasPruebas().Execute()),
                ("Roles", () => new RolesPruebas().Execute()),
                ("Turnos", () => new TurnosPruebas().Execute()),
                ("Usuarios", () => new UsuariosPruebas().Execute()),
            };

            int pass = 0, fail = 0;
            foreach (var (name, action) in testClasses)
            {
                try
                {
                    action();
                    Console.WriteLine($"[PASS] {name}");
                    pass++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FAIL] {name}: {ex.Message}");
                    fail++;
                }
            }

            Console.WriteLine($"\nResultado: {pass} pasadas, {fail} falladas de {testClasses.Length}");
        }
    }
}
