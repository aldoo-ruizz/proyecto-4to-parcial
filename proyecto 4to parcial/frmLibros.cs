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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace proyecto_4to_parcial
{
    public partial class frmLibros : Form
    {
        frmPrincipal objform1 = new frmPrincipal();
        List<Libro> lista = new List<Libro>();

        string ruta = "Libros.txt";
        string rutaPrestamos = "prestamos.txt";

        int idSeleccionado = 0;

        public frmLibros(frmPrincipal formulario1)
        {
            InitializeComponent();
            objform1 = formulario1;

            dgvLibros.SelectionChanged += dgvLibros_SelectionChanged;
        }

        private void frmLibros_Load(object sender, EventArgs e)
        {
            cargararchivo();
            inicializargrid();
            limpiar();
        }

        private void inicializargrid()
        {
            bs2.DataSource = lista;
            dgvLibros.DataSource = bs2;

            dgvLibros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLibros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLibros.MultiSelect = false;
            dgvLibros.ReadOnly = true;
        }

        private void refrescar()
        {
            bs2.DataSource = null;
            bs2.DataSource = lista;
            dgvLibros.DataSource = bs2;
        }

        private void cargararchivo()
        {
            lista.Clear();

            if (File.Exists(ruta))
            {
                string[] lineas = File.ReadAllLines(ruta);

                foreach (string linea in lineas)
                {
                    if (linea.Trim() == "")
                    {
                        continue;
                    }

                    string[] datos = linea.Split('|');

                    if (datos.Length >= 6)
                    {
                        lista.Add(new Libro(
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
                lista.Add(new Libro(1, "Cien años de soledad", "Gabriel García Márquez", "9780307474728", 1967, true));
                lista.Add(new Libro(2, "El principito", "Antoine de Saint-Exupéry", "9780156012195", 1943, true));
                lista.Add(new Libro(3, "Don Quijote de la Mancha", "Miguel de Cervantes", "9788424117502", 1605, true));

                guardararchivo();
            }
        }

        private void guardararchivo()
        {
            StreamWriter sw = new StreamWriter(ruta, false);

            foreach (Libro item in lista)
            {
                sw.WriteLine(item.Id + "|" + item.Titulo + "|" + item.Autor + "|" + item.ISBN + "|" + item.Año + "|" + item.Disponible);
            }

            sw.Close();
        }

        private int siguienteId()
        {
            if (lista.Count == 0)
            {
                return 1;
            }

            return lista.Max(x => x.Id) + 1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (validar(0) == false)
            {
                return;
            }

            Libro nuevo = new Libro(
                siguienteId(),
                txbTitulo.Text,
                txbAutor.Text,
                txbISBN.Text,
                int.Parse(txbAño.Text),
                chbDisp.Checked
            );

            lista.Add(nuevo);

            guardararchivo();
            refrescar();
            limpiar();

            MessageBox.Show("Libro agregado correctamente");
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un libro");
                return;
            }

            if (validar(idSeleccionado) == false)
            {
                return;
            }

            Libro libro = lista.FirstOrDefault(x => x.Id == idSeleccionado);

            if (libro != null)
            {
                libro.Titulo = txbTitulo.Text;
                libro.Autor = txbAutor.Text;
                libro.ISBN = txbISBN.Text;
                libro.Año = int.Parse(txbAño.Text);
                libro.Disponible = chbDisp.Checked;

                guardararchivo();
                refrescar();
                limpiar();

                MessageBox.Show("Libro modificado correctamente");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string buscar = txbBuscar.Text.ToLower();

            var resultado = lista.Where(x =>
                x.Titulo.ToLower().Contains(buscar) ||
                x.Autor.ToLower().Contains(buscar)).ToList();

            bs2.DataSource = null;
            bs2.DataSource = resultado;
            dgvLibros.DataSource = bs2;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un libro");
                return;
            }

            if (tienePrestamoActivo(idSeleccionado))
            {
                MessageBox.Show("No puede eliminar este libro porque tiene préstamo activo");
                return;
            }

            Libro libro = lista.FirstOrDefault(x => x.Id == idSeleccionado);

            if (libro != null)
            {
                lista.Remove(libro);

                guardararchivo();
                refrescar();
                limpiar();

                MessageBox.Show("Libro eliminado correctamente");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            refrescar();
            limpiar();
        }

        private void dgvLibros_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLibros.CurrentRow == null)
            {
                return;
            }

            Libro libro = dgvLibros.CurrentRow.DataBoundItem as Libro;

            if (libro == null)
            {
                return;
            }

            idSeleccionado = libro.Id;

            txbTitulo.Text = libro.Titulo;
            txbAutor.Text = libro.Autor;
            txbISBN.Text = libro.ISBN;
            txbAño.Text = libro.Año.ToString();
            chbDisp.Checked = libro.Disponible;
        }

        private bool validar(int idActual)
        {
            if (txbTitulo.Text.Trim() == "" ||
                txbAutor.Text.Trim() == "" ||
                txbISBN.Text.Trim() == "" ||
                txbAño.Text.Trim() == "")
            {
                MessageBox.Show("Título, Autor, ISBN y Año no pueden estar vacíos");
                return false;
            }

            int año;

            if (int.TryParse(txbAño.Text, out año) == false)
            {
                MessageBox.Show("El año debe ser numérico");
                return false;
            }

            if (año < 1900 || año > DateTime.Today.Year)
            {
                MessageBox.Show("El año debe estar entre 1900 y el año actual");
                return false;
            }

            bool isbnExiste = lista.Any(x =>
                x.ISBN.ToLower() == txbISBN.Text.ToLower() &&
                x.Id != idActual);

            if (isbnExiste)
            {
                MessageBox.Show("El ISBN no puede repetirse");
                return false;
            }

            return true;
        }

        private bool tienePrestamoActivo(int idLibro)
        {
            if (!File.Exists(rutaPrestamos))
            {
                return false;
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
                    int idLibroPrestamo = int.Parse(datos[1]);
                    string estado = datos[5];

                    if (idLibroPrestamo == idLibro && estado != "Devuelto")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void limpiar()
        {
            idSeleccionado = 0;

            txbTitulo.Clear();
            txbAutor.Clear();
            txbISBN.Clear();
            txbAño.Clear();
            txbBuscar.Clear();

            chbDisp.Checked = true;

            dgvLibros.ClearSelection();
        }
    }
}
