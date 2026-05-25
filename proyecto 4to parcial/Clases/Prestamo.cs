using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_4to_parcial.Clases
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int IdLibro { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }

        public Prestamo(int id, int idLibro, int idSocio, DateTime fechaPrestamo, DateTime? fechaDevolucion, string estado)
        {
            Id = id;
            IdLibro = idLibro;
            IdSocio = idSocio;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucion = fechaDevolucion;
            Estado = estado;
        }
    }
}
