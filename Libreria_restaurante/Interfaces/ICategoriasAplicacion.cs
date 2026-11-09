using Libreria_restaurante.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Interfaces
{
    public class ICategoriasAplicacion
    {
        Categorias Insertar(Categorias entidad);
        Categorias Actualizar(Categorias entidad);
        List<Categorias> Consultar();
        List<Categorias> Buscar(Categorias entidad);
        Categorias Borrar(Categorias entidad);
    }
}