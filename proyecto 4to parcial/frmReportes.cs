using proyecto_4to_parcial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace proyecto_4to_parcial
{
    public partial class frmReportes : Form
    {
        string rutaLibros = "Libros.txt";
        string rutaSocios = "Socios.txt";
        string rutaPrestamos = "Prestamos.txt";

        public frmReportes()
        {
            InitializeComponent();

            this.Load += frmReportes_Load;
            btnLibrosPrestados.Click += btnLibrosPrestados_Click;
            btnSociosActivos.Click += btnSociosActivos_Click;
            btnPrestamosActivos.Click += btnPrestamosActivos_Click;
            btnLibrosDisponibles.Click += btnLibrosDisponibles_Click;
            btnSociossPrestamos.Click += btnSociossPrestamos_Click;
        }

        private void frmReportes_Load(object sender, EventArgs e)
        {
            dgvReportes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReportes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReportes.MultiSelect = false;
            dgvReportes.ReadOnly = true;
        }

        private List<Libro> cargarLibros()
        {
            List<Libro> libros = new List<Libro>();

            if (!File.Exists(rutaLibros))
            {
                return libros;
            }

            string[] lineas = File.ReadAllLines(rutaLibros);

            foreach (string linea in lineas)
            {
                if (linea.Trim() == "")
                {
                    continue;
                }

                string[] datos = linea.Split('|');

                if (datos.Length >= 6)
                {
                    libros.Add(new Libro(
                        int.Parse(datos[0]),
                        datos[1],
                        datos[2],
                        datos[3],
                        int.Parse(datos[4]),
                        bool.Parse(datos[5])
                    ));
                }
            }

            return libros;
        }

        private List<Socio> cargarSocios()
        {
            List<Socio> socios = new List<Socio>();

            if (!File.Exists(rutaSocios))
            {
                return socios;
            }

            string[] lineas = File.ReadAllLines(rutaSocios);

            foreach (string linea in lineas)
            {
                if (linea.Trim() == "")
                {
                    continue;
                }

                string[] datos = linea.Split('|');

                if (datos.Length >= 6)
                {
                    socios.Add(new Socio(
                        int.Parse(datos[0]),
                        datos[1],
                        datos[2],
                        datos[3],
                        datos[4],
                        DateTime.Parse(datos[5])
                    ));
                }
            }

            return socios;
        }

        private List<Prestamo> cargarPrestamos()
        {
            List<Prestamo> prestamos = new List<Prestamo>();

            if (!File.Exists(rutaPrestamos))
            {
                return prestamos;
            }

            string[] lineas = File.ReadAllLines(rutaPrestamos);

            foreach (string linea in lineas)
            {
                if (linea.Trim() == "")
                {
                    continue;
                }

                string[] datos = linea.Split('|');

                if (datos.Length >= 6)
                {
                    DateTime? fechaDevolucion = null;

                    if (datos[4] != "")
                    {
                        fechaDevolucion = DateTime.Parse(datos[4]);
                    }

                    prestamos.Add(new Prestamo(
                        int.Parse(datos[0]),
                        int.Parse(datos[1]),
                        int.Parse(datos[2]),
                        DateTime.Parse(datos[3]),
                        fechaDevolucion,
                        datos[5]
                    ));
                }
            }

            return prestamos;
        }

        private void btnLibrosPrestados_Click(object sender, EventArgs e)
        {
            List<Libro> libros = cargarLibros();
            List<Prestamo> prestamos = cargarPrestamos();

            var consulta = prestamos
                .GroupBy(x => x.IdLibro)
                .Select(g => new
                {
                    IdLibro = g.Key,
                    Libro = libros.FirstOrDefault(l => l.Id == g.Key) == null
                        ? "No encontrado"
                        : libros.FirstOrDefault(l => l.Id == g.Key).Titulo,
                    TotalPrestamos = g.Count()
                })
                .OrderByDescending(x => x.TotalPrestamos)
                .Take(5)
                .ToList();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = consulta;
        }

        private void btnSociosActivos_Click(object sender, EventArgs e)
        {
            List<Socio> socios = cargarSocios();
            List<Prestamo> prestamos = cargarPrestamos();

            var consulta = prestamos
                .GroupBy(x => x.IdSocio)
                .Select(g => new
                {
                    IdSocio = g.Key,
                    Socio = socios.FirstOrDefault(s => s.Id == g.Key) == null
                        ? "No encontrado"
                        : socios.FirstOrDefault(s => s.Id == g.Key).Nombre,
                    TotalPrestamos = g.Count()
                })
                .OrderByDescending(x => x.TotalPrestamos)
                .Take(5)
                .ToList();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = consulta;
        }

        private void btnPrestamosActivos_Click(object sender, EventArgs e)
        {
            List<Libro> libros = cargarLibros();
            List<Socio> socios = cargarSocios();
            List<Prestamo> prestamos = cargarPrestamos();

            var consulta = from p in prestamos
                           join l in libros on p.IdLibro equals l.Id
                           join s in socios on p.IdSocio equals s.Id
                           where p.Estado != "Devuelto"
                           orderby p.FechaPrestamo
                           select new
                           {
                               Id = p.Id,
                               Libro = l.Titulo,
                               Socio = s.Nombre,
                               FechaPrestamo = p.FechaPrestamo.ToString("yyyy-MM-dd"),
                               Dias = (DateTime.Today - p.FechaPrestamo.Date).Days,
                               Estado = p.Estado
                           };

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = consulta.ToList();
        }

        private void btnLibrosDisponibles_Click(object sender, EventArgs e)
        {
            List<Libro> libros = cargarLibros();

            var consulta = libros
                .Where(x => x.Disponible == true)
                .OrderBy(x => x.Titulo)
                .Select(x => new
                {
                    x.Id,
                    x.Titulo,
                    x.Autor,
                    x.ISBN,
                    x.Año
                })
                .ToList();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = consulta;
        }

        private void btnSociossPrestamos_Click(object sender, EventArgs e)
        {
            List<Socio> socios = cargarSocios();
            List<Prestamo> prestamos = cargarPrestamos();

            var consulta = socios
                .Where(s => !prestamos.Any(p => p.IdSocio == s.Id))
                .OrderBy(s => s.Nombre)
                .Select(s => new
                {
                    s.Id,
                    s.Nombre,
                    s.Telefono,
                    s.Email,
                    FechaRegistro = s.FechaRegistro.ToString("yyyy-MM-dd")
                })
                .ToList();

            dgvReportes.DataSource = null;
            dgvReportes.DataSource = consulta;
        }
    }
}
