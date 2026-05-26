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
    /// <summary>
    /// ARD 25052026
    /// En este formulario se pueden administrar los socios, se pueden agregar, modificar, eliminar y buscar socios la información se guarda y se carga desde un archivo txt, también valida que no existan emails repetidos, que el email tenga formato correcto, que el teléfono tenga al menos 10 dígitos y que no se puedan eliminar socios con préstamos activos.
    /// </summary>
    public partial class frmSocios : Form
    {
        frmPrincipal objform1 = new frmPrincipal();

        List<Socio> lista = new List<Socio>();

        string ruta = "Socios.txt";
        string rutaPrestamos = "Prestamos.txt";

        int idSeleccionado = 0;

        public frmSocios(frmPrincipal formulario1)
        {
            InitializeComponent();

            objform1 = formulario1;

            this.Load += frmSocios_Load;

            btnAgregar.Click += btnAgregar_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;

            dgvSocios.SelectionChanged += dgvSocios_SelectionChanged;
        }

        private void frmSocios_Load(object sender, EventArgs e)
        {
            cargararchivo();
            inicializargrid();
            limpiar();
        }

        private void inicializargrid()
        {
            dgvSocios.DataSource = lista;

            dgvSocios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSocios.MultiSelect = false;
            dgvSocios.ReadOnly = true;
        }

        private void refrescar()
        {
            dgvSocios.DataSource = null;
            dgvSocios.DataSource = lista;
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
                        lista.Add(new Socio(int.Parse(datos[0]), datos[1], datos[2], datos[3], datos[4], DateTime.Parse(datos[5])));
                    }
                }
            }
            else
            {
                lista.Add(new Socio(1, "Ana López", "Calle Reforma 10", "7771234567", "ana@gmail.com", DateTime.Today));
                lista.Add(new Socio(2, "Carlos Pérez", "Av. Morelos 25", "7777654321", "carlos@gmail.com", DateTime.Today));

                guardararchivo();
            }
        }

        private void guardararchivo()
        {
            StreamWriter sw = new StreamWriter(ruta, false);

            foreach (Socio item in lista)
            {
                sw.WriteLine(item.Id + "|" + item.Nombre + "|" + item.Direccion + "|" + item.Telefono + "|" + item.Email + "|" + item.FechaRegistro.ToString("yyyy-MM-dd"));
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

            Socio nuevo = new Socio(siguienteId(), txbNombre.Text, txbDireccion.Text, txbTelefono.Text, txbEmail.Text, dtpFecha.Value.Date);

            lista.Add(nuevo);

            guardararchivo();
            refrescar();
            limpiar();

            MessageBox.Show("Socio agregado correctamente");
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un socio");
                return;
            }

            if (validar(idSeleccionado) == false)
            {
                return;
            }   

            Socio socio = lista.FirstOrDefault(x => x.Id == idSeleccionado);

            if (socio != null)
            {
                socio.Nombre = txbNombre.Text;
                socio.Direccion = txbDireccion.Text;
                socio.Telefono = txbTelefono.Text;
                socio.Email = txbEmail.Text;
                socio.FechaRegistro = dtpFecha.Value.Date;

                guardararchivo();
                refrescar();
                limpiar();

                MessageBox.Show("Socio modificado correctamente");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un socio");
                return;
            }

            if (tienePrestamoActivo(idSeleccionado))
            {
                MessageBox.Show("No puede eliminar este socio porque tiene préstamo activo");
                return;
            }

            Socio socio = lista.FirstOrDefault(x => x.Id == idSeleccionado);

            if (socio != null)
            {
                lista.Remove(socio);

                guardararchivo();
                refrescar();
                limpiar();

                MessageBox.Show("Socio eliminado correctamente");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string buscar = txbBuscar.Text.ToLower();

            var resultado = lista.Where(x => x.Nombre.ToLower().Contains(buscar) || x.Email.ToLower().Contains(buscar)).ToList();

            dgvSocios.DataSource = null;
            dgvSocios.DataSource = resultado;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            refrescar();
            limpiar();
        }

        private void dgvSocios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSocios.CurrentRow == null)
            {
                return;
            }

            Socio socio = dgvSocios.CurrentRow.DataBoundItem as Socio;

            if (socio == null)
            {
                return;
            }

            idSeleccionado = socio.Id;

            txbNombre.Text = socio.Nombre;
            txbDireccion.Text = socio.Direccion;
            txbTelefono.Text = socio.Telefono;
            txbEmail.Text = socio.Email;

            dtpFecha.Value = socio.FechaRegistro;
        }

        private bool validar(int idActual)
        {
            if (txbNombre.Text.Trim() == "" || txbDireccion.Text.Trim() == "" || txbTelefono.Text.Trim() == "" || txbEmail.Text.Trim() == "")
            {
                MessageBox.Show("Nombre, Dirección, Teléfono y Email no pueden estar vacíos");
                return false;
            }

            if (txbEmail.Text.Contains("@") == false)
            {
                MessageBox.Show("El email debe contener @");
                return false;
            }

            int digitos = txbTelefono.Text.Count(char.IsDigit);

            if (digitos < 10)
            {
                MessageBox.Show("El teléfono debe tener al menos 10 dígitos");
                return false;
            }

            bool emailExiste = lista.Any(x => x.Email.ToLower() == txbEmail.Text.ToLower() && x.Id != idActual);

            if (emailExiste)
            {
                MessageBox.Show("El email no puede repetirse");
                return false;
            }

            return true;
        }

        private bool tienePrestamoActivo(int idSocio)
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
                    int idSocioPrestamo = int.Parse(datos[2]);
                    string estado = datos[5];

                    if (idSocioPrestamo == idSocio && estado != "Devuelto")
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

            txbNombre.Clear();
            txbDireccion.Clear();
            txbTelefono.Clear();
            txbEmail.Clear();
            txbBuscar.Clear();

            dtpFecha.Value = DateTime.Today;

            dgvSocios.ClearSelection();
        }

    }
}

