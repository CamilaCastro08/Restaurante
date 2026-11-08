using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IGerentesPresentacion
    {
        Gerentes Insertar(Gerentes entidad);
        Gerentes Actualizar(Gerentes entidad);
        List<Gerentes> Consultar();
        List<Gerentes> Buscar(Gerentes entidad);
        Gerentes Borrar(Gerentes entidad);
    }
}
