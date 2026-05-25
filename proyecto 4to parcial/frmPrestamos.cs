using proyecto_4to_parcial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyecto_4to_parcial
{
    public partial class frmPrestamos : Form
    {
        List<Libro> libros = new List<Libro>();
        List<Socio> socios = new List<Socio>();
        List<Prestamo> prestamos = new List<Prestamo>();

        string rutaLibros = "Libros.txt";
        string rutaSocios = "Socios.txt";
        string rutaPrestamos = "Prestamos.txt";

        public frmPrestamos()
        {
            InitializeComponent();

            this.Load += frmPrestamos_Load;
            btnPrestamo.Click += btnPrestamo_Click;
            btnDevolucion.Click += btnDevolucion_Click;
        }

        private void frmPrestamos_Load(object sender, EventArgs e)
        {
            dgvPrestamos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrestamos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPrestamos.MultiSelect = false;
            dgvPrestamos.ReadOnly = true;

            dtpFecha.Value = DateTime.Today;

            cargarTodo();
        }

        private void cargarTodo()
        {
            cargarLibros();
            cargarSocios();
            cargarPrestamos();
            actualizarVencidos();

            cargarCombos();
            mostrarPrestamos();
        }

        private void cargarLibros()
        {
            libros.Clear();

            if (File.Exists(rutaLibros))
            {
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
            }
            else
            {
                libros.Add(new Libro(1, "Cien años de soledad", "Gabriel García Márquez", "9780307474728", 1967, true));
                libros.Add(new Libro(2, "El principito", "Antoine de Saint-Exupéry", "9780156012195", 1943, true));
                libros.Add(new Libro(3, "Don Quijote de la Mancha", "Miguel de Cervantes", "9788424117502", 1605, true));

                guardarLibros();
            }
        }

        private void guardarLibros()
        {
            StreamWriter sw = new StreamWriter(rutaLibros, false);

            foreach (Libro item in libros)
            {
                sw.WriteLine(item.Id + "|" + item.Titulo + "|" + item.Autor + "|" + item.ISBN + "|" + item.Año + "|" + item.Disponible);
            }

            sw.Close();
        }

        private void cargarSocios()
        {
            socios.Clear();

            if (File.Exists(rutaSocios))
            {
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
            }
            else
            {
                socios.Add(new Socio(1, "Ana López", "Calle Reforma 10", "7771234567", "ana@gmail.com", DateTime.Today));
                socios.Add(new Socio(2, "Carlos Pérez", "Av. Morelos 25", "7777654321", "carlos@gmail.com", DateTime.Today));

                guardarSocios();
            }
        }

        private void guardarSocios()
        {
            StreamWriter sw = new StreamWriter(rutaSocios, false);

            foreach (Socio item in socios)
            {
                sw.WriteLine(item.Id + "|" + item.Nombre + "|" + item.Direccion + "|" + item.Telefono + "|" + item.Email + "|" + item.FechaRegistro.ToString("yyyy-MM-dd"));
            }

            sw.Close();
        }

        private void cargarPrestamos()
        {
            prestamos.Clear();

            if (File.Exists(rutaPrestamos))
            {
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
            }
            else
            {
                File.Create(rutaPrestamos).Close();
            }
        }

        private void guardarPrestamos()
        {
            StreamWriter sw = new StreamWriter(rutaPrestamos, false);

            foreach (Prestamo item in prestamos)
            {
                string fechaDevolucion = "";

                if (item.FechaDevolucion != null)
                {
                    fechaDevolucion = item.FechaDevolucion.Value.ToString("yyyy-MM-dd");
                }

                sw.WriteLine(item.Id + "|" + item.IdLibro + "|" + item.IdSocio + "|" + item.FechaPrestamo.ToString("yyyy-MM-dd") + "|" + fechaDevolucion + "|" + item.Estado);
            }

            sw.Close();
        }

        private int siguienteIdPrestamo()
        {
            if (prestamos.Count == 0)
            {
                return 1;
            }

            return prestamos.Max(x => x.Id) + 1;
        }

        private void actualizarVencidos()
        {
            foreach (Prestamo prestamo in prestamos)
            {
                if (prestamo.Estado != "Devuelto")
                {
                    int dias = (DateTime.Today - prestamo.FechaPrestamo.Date).Days;

                    if (dias > 15)
                    {
                        prestamo.Estado = "Vencido";
                    }
                    else
                    {
                        prestamo.Estado = "Activo";
                    }
                }
            }

            guardarPrestamos();
        }

        private void cargarCombos()
        {
            cmbSocio.DataSource = null;
            cmbSocio.DataSource = socios;
            cmbSocio.DisplayMember = "Nombre";
            cmbSocio.ValueMember = "Id";

            cmbLibro.DataSource = null;
            cmbLibro.DataSource = libros.Where(x => x.Disponible == true).ToList();
            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "Id";
        }

        private void mostrarPrestamos()
        {
            var consulta = from p in prestamos
                           join l in libros on p.IdLibro equals l.Id
                           join s in socios on p.IdSocio equals s.Id
                           orderby p.Id descending
                           select new
                           {
                               Id = p.Id,
                               Libro = l.Titulo,
                               Socio = s.Nombre,
                               FechaPrestamo = p.FechaPrestamo.ToString("yyyy-MM-dd"),
                               FechaDevolucion = p.FechaDevolucion == null ? "Pendiente" : p.FechaDevolucion.Value.ToString("yyyy-MM-dd"),
                               Estado = p.Estado
                           };

            dgvPrestamos.DataSource = null;
            dgvPrestamos.DataSource = consulta.ToList();
        }

        private void btnPrestamo_Click(object sender, EventArgs e)
        {
            if (cmbSocio.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un socio");
                return;
            }

            if (cmbLibro.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un libro disponible");
                return;
            }

            Socio socio = cmbSocio.SelectedItem as Socio;
            Libro libro = cmbLibro.SelectedItem as Libro;

            if (socio == null || libro == null)
            {
                MessageBox.Show("Seleccione socio y libro correctamente");
                return;
            }

            int prestamosActivos = prestamos.Count(x =>
                x.IdSocio == socio.Id &&
                x.Estado != "Devuelto");

            if (prestamosActivos >= 3)
            {
                MessageBox.Show("El socio ya tiene 3 préstamos activos");
                return;
            }

            if (libro.Disponible == false)
            {
                MessageBox.Show("El libro no está disponible");
                return;
            }

            Prestamo nuevo = new Prestamo(
                siguienteIdPrestamo(),
                libro.Id,
                socio.Id,
                dtpFecha.Value.Date,
                null,
                "Activo"
            );

            prestamos.Add(nuevo);
            libro.Disponible = false;

            guardarPrestamos();
            guardarLibros();

            cargarTodo();

            MessageBox.Show("Préstamo registrado correctamente");
        }

        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            if (dgvPrestamos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un préstamo");
                return;
            }

            int idPrestamo = Convert.ToInt32(dgvPrestamos.CurrentRow.Cells["Id"].Value);

            Prestamo prestamo = prestamos.FirstOrDefault(x => x.Id == idPrestamo);

            if (prestamo == null)
            {
                MessageBox.Show("No se encontró el préstamo");
                return;
            }

            if (prestamo.Estado == "Devuelto")
            {
                MessageBox.Show("Este préstamo ya fue devuelto");
                return;
            }

            prestamo.FechaDevolucion = DateTime.Today;
            prestamo.Estado = "Devuelto";

            Libro libro = libros.FirstOrDefault(x => x.Id == prestamo.IdLibro);

            if (libro != null)
            {
                libro.Disponible = true;
            }

            guardarPrestamos();
            guardarLibros();

            cargarTodo();

            MessageBox.Show("Devolución registrada correctamente");
        }
    }

}

