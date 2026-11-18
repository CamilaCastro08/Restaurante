using Lib_Presentaciones.Implementaciones;
using Lib_Presentaciones.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});
builder.Services.AddScoped<ICategoriasPresentacion, CategoriasPresentacion>();
builder.Services.AddScoped<IClientesPresentacion, ClientesPresentacion>();
builder.Services.AddScoped<ICocinerosPresentacion, CocinerosPresentacion>();
builder.Services.AddScoped<IDetalles_PedidosPresentacion, Detalles_PedidosPresentacion>();
builder.Services.AddScoped<IEmpleadosPresentacion, EmpleadosPresentacion>();
builder.Services.AddScoped<IFacturasPresentacion, FacturasPresentacion>();
builder.Services.AddScoped<IGerentesPresentacion, GerentesPresentacion>();
builder.Services.AddScoped<IIngredientesPresentacion, IngredientesPresentacion>();
builder.Services.AddScoped<IInventariosPresentacion, InventariosPresentacion>();
builder.Services.AddScoped<IMenusPresentacion, MenusPresentacion>();
builder.Services.AddScoped<IMesasPresentacion, MesasPresentacion>();
builder.Services.AddScoped<IMeserosPresentacion, MeserosPresentacion>();
builder.Services.AddScoped<IMetodosPagosPresentacion, MetodosPagosPresentacion>();
builder.Services.AddScoped<IPagosPresentacion, PagosPresentacion>();
builder.Services.AddScoped<IPedidosPresentacion, PedidosPresentacion>();
builder.Services.AddScoped<IProductosPresentacion, ProductosPresentacion>();
builder.Services.AddScoped<IProveedoresPresentacion, ProveedoresPresentacion>();
builder.Services.AddScoped<IRecetasPresentacion, RecetasPresentacion>();
builder.Services.AddScoped<IReservasPresentacion, ReservasPresentacion>();
builder.Services.AddScoped<ITurnosPresentacion, TurnosPresentacion>();
builder.Services.AddScoped<IRolesPresentacion, RolesPresentacion>();
builder.Services.AddScoped<IUsuariosPresentacion, UsuariosPresentacion>();
builder.Services.AddScoped<IPermisosPresentacion, PermisosPresentacion>();
builder.Services.AddScoped<IAuditoriasPresentacion, AuditoriasPresentacion>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
