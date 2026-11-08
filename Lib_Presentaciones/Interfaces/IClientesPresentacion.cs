using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IClientesPresentacion
    {
        Clientes Insertar(Clientes entidad);
        Clientes Actualizar(Clientes entidad);
        List<Clientes> Consultar();
        List<Clientes> Buscar(Clientes entidad);
        Clientes Borrar(Clientes entidad);
    }
}
