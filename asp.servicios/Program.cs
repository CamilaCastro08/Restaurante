using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IConexion>(x =>
{
    var conexion = new Conexion();
    conexion.StringConexion = builder.Configuration.GetConnectionString("string_conexion");
    return conexion;
});
builder.Services.AddScoped<ICategoriasAplicacion, CategoriasAplicacion>();
builder.Services.AddScoped<IRolesAplicacion, RolesAplicacion>();
builder.Services.AddScoped<IPermisosAplicacion, PermisosAplicacion>();
builder.Services.AddScoped<IUsuariosAplicacion, UsuariosAplicacion>();
builder.Services.AddScoped<IClientesAplicacion, ClientesAplicacion>();
builder.Services.AddScoped<ICocinerosAplicacion, CocinerosAplicacion>();
builder.Services.AddScoped<IDetalles_PedidosAplicacion, Detalles_PedidosAplicacion>();
builder.Services.AddScoped<IEmpleadosAplicacion, EmpleadosAplicacion>();
builder.Services.AddScoped<IFacturasAplicacion, FacturasAplicacion>();
builder.Services.AddScoped<IGerentesAplicacion, GerentesAplicacion>();
builder.Services.AddScoped<IIngredientesAplicacion, IngredientesAplicacion>();
builder.Services.AddScoped<IInventariosAplicacion, InventariosAplicacion>();
builder.Services.AddScoped<IMenusAplicacion, MenusAplicacion>();
builder.Services.AddScoped<IMesasAplicacion, MesasAplicacion>();
builder.Services.AddScoped<IMeserosAplicacion, MeserosAplicacion>();
builder.Services.AddScoped<IMetodosPagosAplicacion, MetodosPagosAplicacion>();
builder.Services.AddScoped<IPagosAplicacion, PagosAplicacion>();
builder.Services.AddScoped<IPedidosAplicacion, PedidosAplicacion>();
builder.Services.AddScoped<IProductosAplicacion, ProductosAplicacion>();
builder.Services.AddScoped<IProveedoresAplicacion, ProveedoresAplicacion>();
builder.Services.AddScoped<IRecetasAplicacion, RecetasAplicacion>();
builder.Services.AddScoped<IReservasAplicacion, ReservasAplicacion>();
builder.Services.AddScoped<ITurnosAplicacion, TurnosAplicacion>();
builder.Services.AddScoped<IAuditoriasAplicacion, AuditoriasAplicacion>();

var app = builder.Build();
app.UseExceptionHandler(a => a.Run(async context =>
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    context.Response.StatusCode = 400;
    context.Response.ContentType = "text/plain; charset=utf-8";
    await context.Response.WriteAsync(error != null && error.GetType() == typeof(Exception) ? error.Message : "Error en el servidor");
}));
app.MapControllers();
app.Run();
