using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class IAuditoriasAplicacion
    {
        List<Auditorias> Consultar();
        Auditorias Actualizar(Auditorias entidad);
    }
}