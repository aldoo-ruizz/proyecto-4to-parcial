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
    public partial class frmLibros : Form
    {
        frmPrincipal objform1 = new frmPrincipal();
        List<Libro> lista = new List<Libro>();
        string ruta = "Libros.txt";


        public frmLibros(frmPrincipal formulario1)
        {
            InitializeComponent();
            objform1 = formulario1;

        }

        private void frmLibros_Load(object sender, EventArgs e)
        {
            bs2.DataSource = lista;
            dgvLibros.DataSource = bs2;

            dgvLibros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLibros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLibros.MultiSelect = false;
            dgvLibros.ReadOnly = true;
        }
        private void cargararchivo()
        {
            if (File.Exists(ruta))
            {
                string[] lineas = File.ReadAllLines(ruta);

                foreach (string linea in lineas)
                {
                    string[] datos = linea.Split('|');

                    lista.Add(new Libro(int.Parse(datos[0]), datos[1], datos[2], datos[3], int.Parse(datos[4]), bool.Parse(datos[5])));
                }
            }
            else
            {
                lista.Add(new Libro(1, "Frutas", "Juan Pérez", "978-0-123456-78-9", 2020, true));
                lista.Add(new Libro(2, "Carne", "María García", "978-0-987654-32-1", 2020, true));
                lista.Add(new Libro(3, "Verduras", "Pedro López", "978-0-567890-12-3", 2020, true));
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            StreamWriter sw = new StreamWriter(ruta);

            foreach (Libro item in lista)
            {
                sw.WriteLine(item.Id + "|" + item.Titulo + "|" + item.Autor + "|" + item.ISBN + "|" + item.Año + "|" + item.Disponible);
            }

            sw.Close();

            MessageBox.Show("Datos guardados correctamente");

            Application.Exit();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

        }
    }
}
