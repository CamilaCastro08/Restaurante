using Libreria_restaurante.Implementaciones;
using Libreria_restaurante.Interfaces;
using Libreria_restaurante.Nucleo;

try
{
    IConexion conexion = new Conexion();
    conexion.StringConexion = MetodosGenerales.ObtenerStringConexion();
    var lista = conexion.Categorias!.ToList();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
