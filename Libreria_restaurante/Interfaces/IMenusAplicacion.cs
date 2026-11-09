using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IMenusAplicacion
    {
        Menus Insertar(Menus entidad);
        Menus Actualizar(Menus entidad);
        List<Menus> Consultar();
        List<Menus> Buscar(Menus entidad);
        Menus Borrar(Menus entidad);
    }
}