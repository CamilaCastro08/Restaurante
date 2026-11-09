using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IMenusPresentacion
    {
        IMenus Insertar(IMenus entidad);
        IMenus Actualizar(IMenus entidad);
        List<IMenus> Consultar();
        List<IMenus> Buscar(IMenus entidad);
        IMenus Borrar(IMenus entidad);
    }
}
