using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib_Presentaciones.Interfaces
{
    public class IIngredientesPresentacion
    {
        Ingredientes Insertar(Ingredientes entidad);
        Ingredientes Actualizar(Ingredientes entidad);
        List<Ingredientes> Consultar();
        List<Ingredientes> Buscar(Ingredientes entidad);
        Ingredientes Borrar(Ingredientes entidad);
    }
}
