using proyecto_4to_parcial.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace proyecto_4to_parcial
{
    /// <summary>
    /// ARD 25052026
    /// En este formulario se pueden generar diferentes reportes relacionados con los libros, socios y préstamos, como los libros más prestados, los socios más activos, los préstamos activos, los libros disponibles y los socios sin préstamos. La información se carga desde archivos txt y se muestra en un DataGridView para facilitar su visualización.
    /// </summary>
    public partial class frmReportes : Form
    {
        frmPrincipal objform1 = new frmPrincipal();

    string rutaLibros = "Libros.txt";
    string rutaSocios = "Socios.txt";
    string rutaPrestamos = "Prestamos.txt";

    public frmReportes(frmPrincipal formulario1)
    {
        InitializeComponent();

        objform1 = formulario1;

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

        if (File.Exists(rutaLibros))
        {
            string[] lineas = File.ReadAllLines(rutaLibros);

            foreach (string linea in lineas)
            {
                if (linea.Trim() != "")
                {
                    string[] datos = linea.Split('|');

                    if (datos.Length >= 6)
                    {
                        libros.Add(new Libro(int.Parse(datos[0]), datos[1], datos[2], datos[3], int.Parse(datos[4]), bool.Parse(datos[5])));
                    }
                }
            }
        }

        return libros;
    }

    private List<Socio> cargarSocios()
    {
        List<Socio> socios = new List<Socio>();

        if (File.Exists(rutaSocios))
        {
            string[] lineas = File.ReadAllLines(rutaSocios);

            foreach (string linea in lineas)
            {
                if (linea.Trim() != "")
                {
                    string[] datos = linea.Split('|');

                    if (datos.Length >= 6)
                    {
                        socios.Add(new Socio(int.Parse(datos[0]), datos[1], datos[2], datos[3], datos[4], DateTime.Parse(datos[5])));
                    }
                }
            }
        }

        return socios;
    }

    private List<Prestamo> cargarPrestamos()
    {
        List<Prestamo> prestamos = new List<Prestamo>();

        if (File.Exists(rutaPrestamos))
        {
            string[] lineas = File.ReadAllLines(rutaPrestamos);

            foreach (string linea in lineas)
            {
                if (linea.Trim() != "")
                {
                    string[] datos = linea.Split('|');

                    if (datos.Length >= 6)
                    {
                        DateTime? fechaDevolucion = null;

                        if (datos[4] != "")
                        {
                            fechaDevolucion = DateTime.Parse(datos[4]);
                        }

                        prestamos.Add(new Prestamo(int.Parse(datos[0]), int.Parse(datos[1]), int.Parse(datos[2]), DateTime.Parse(datos[3]), fechaDevolucion, datos[5]));
                    }
                }
            }
        }

        return prestamos;
    }

    private void btnLibrosPrestados_Click(object sender, EventArgs e)
    {
        List<Libro> libros = cargarLibros();
        List<Prestamo> prestamos = cargarPrestamos();

        var consulta = prestamos.GroupBy(x => x.IdLibro)
        .Select(x => new
        {
            IdLibro = x.Key,
            Libro = libros.FirstOrDefault(l => l.Id == x.Key)?.Titulo ?? "No encontrado",
            TotalPrestamos = x.Count()
        })
        .OrderByDescending(x => x.TotalPrestamos)
        .Take(5)
        .ToList();

        dgvReportes.DataSource = consulta;
    }

    private void btnSociosActivos_Click(object sender, EventArgs e)
    {
        List<Socio> socios = cargarSocios();
        List<Prestamo> prestamos = cargarPrestamos();

        var consulta = prestamos.GroupBy(x => x.IdSocio)
        .Select(x => new
        {
            IdSocio = x.Key,
            Socio = socios.FirstOrDefault(s => s.Id == x.Key)?.Nombre ?? "No encontrado",
            TotalPrestamos = x.Count()
        })
        .OrderByDescending(x => x.TotalPrestamos)
        .Take(5)
        .ToList();

        dgvReportes.DataSource = consulta;
    }

    private void btnPrestamosActivos_Click(object sender, EventArgs e)
    {
        List<Libro> libros = cargarLibros();
        List<Socio> socios = cargarSocios();
        List<Prestamo> prestamos = cargarPrestamos();

            List<object> consulta = new List<object>();

            foreach (Prestamo p in prestamos)
            {
                if (p.Estado != "Devuelto")
                {
                    Libro libro = libros.FirstOrDefault(x => x.Id == p.IdLibro);
                    Socio socio = socios.FirstOrDefault(x => x.Id == p.IdSocio);

                    consulta.Add(new{p.Id,Libro = libro.Titulo,Socio = socio.Nombre,FechaPrestamo = p.FechaPrestamo.ToString("yyyy-MM-dd"),Dias = (DateTime.Today - p.FechaPrestamo.Date).Days,p.Estado});
                }
            }

            dgvReportes.DataSource = consulta;
        }

    private void btnLibrosDisponibles_Click(object sender, EventArgs e)
    {
        List<Libro> libros = cargarLibros();

        var consulta = libros.Where(x => x.Disponible)
        .Select(x => new
        {
            x.Id,
            x.Titulo,
            x.Autor,
            x.ISBN,
            x.Año
        })
        .ToList();

        dgvReportes.DataSource = consulta;
    }

    private void btnSociossPrestamos_Click(object sender, EventArgs e)
    {
        List<Socio> socios = cargarSocios();
        List<Prestamo> prestamos = cargarPrestamos();

        var consulta = socios.Where(s => !prestamos.Any(p => p.IdSocio == s.Id))
        .Select(s => new
        {
            s.Id,
            s.Nombre,
            s.Telefono,
            s.Email,
            FechaRegistro = s.FechaRegistro.ToString("yyyy-MM-dd")
        })
        .ToList();

        dgvReportes.DataSource = consulta;
    }
}
}
