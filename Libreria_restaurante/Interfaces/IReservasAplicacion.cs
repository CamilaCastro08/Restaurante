using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IReservasAplicacion
    {
        Reservas Insertar(Reservas entidad);
        Reservas Actualizar(Reservas entidad);
        List<Reservas> Consultar();
        List<Reservas> Buscar(Reservas entidad);
        Reservas Borrar(Reservas entidad);
    }
}