using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IRecetasPresentacion
    {
        Recetas Insertar(Recetas entidad);
        Recetas Actualizar(Recetas entidad);
        List<Recetas> Consultar();
        List<Recetas> Buscar(Recetas entidad);
        Recetas Borrar(Recetas entidad);
    }
}
