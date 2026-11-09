using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IGerentes
    {
        Gerentes Insertar(Gerentes entidad);
        Gerentes Actualizar(Gerentes entidad);
        List<Gerentes> Consultar();

        List<Gerentes> Buscar(Gerentes entidad);
        Gerentes Borrar(Gerentes entidad);
    }
}