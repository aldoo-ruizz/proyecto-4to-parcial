using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_4to_parcial.Clases
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public int Año { get; set; }
        public bool Disponible { get; set; }


        public Libro(int id, string titulo, string autor,string isbn, int año, bool disponible)
        {
            Id = id;
            Titulo = titulo;
            Autor = autor;
            ISBN = isbn;
            Año = año;
            Disponible = disponible;
        }
    }
}
