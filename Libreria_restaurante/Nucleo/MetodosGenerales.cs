using System;
using System.Collections.Generic;
using System.Text;

namespace Libreria_restaurante.Nucleo
{
   public class MetodosGenerales
    {
        public static string ObtenerStringConexion(string v)
        {
            return "server=localhost;database=db_Restaurante;Integrated Security=True;TrustServerCertificate=true;";
        }
    }
}
