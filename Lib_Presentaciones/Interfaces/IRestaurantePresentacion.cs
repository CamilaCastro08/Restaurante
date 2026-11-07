using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public interface IRestaurantePresentacion
    {
        Animales Insertar(Animales entidad);
        Animales Actualizar(Animales entidad);
        List<Animales> Consultar();
        List<Animales> Buscar(Animales entidad);
        Animales Borrar(Animales entidad);
    }
}
