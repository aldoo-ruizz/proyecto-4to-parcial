using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyecto_4to_parcial
{
    /// <summary>
    /// ARD 25052026
    /// En este formulario se encuentra el menú principal de la aplicación, desde donde se pueden acceder a los diferentes formularios para administrar libros, socios, préstamos y reportes. También se incluye una opción para salir de la aplicación. El formulario se configura como MDI para permitir la apertura de múltiples formularios hijos dentro de la misma ventana principal.
    /// </summary>
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

      

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void librosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formulario = Application.OpenForms["frmLibros"];

            if (formulario != null)
            {
                formulario.Activate();
                return;
            }

            frmLibros frmlibros = new frmLibros(this);
            frmlibros.MdiParent = this;
            frmlibros.Show();
        }

        private void sociosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formulario = Application.OpenForms["frmSocios"];

            if (formulario != null)
            {
                formulario.Activate();
                return;
            }

            frmSocios frmsocios = new frmSocios(this);
            frmsocios.MdiParent = this;
            frmsocios.Show();
        }

        private void prestamosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formulario = Application.OpenForms["frmPrestamos"];

            if (formulario != null)
            {
                formulario.Activate();
                return;
            }

            frmPrestamos frmprestamos = new frmPrestamos(this);
            frmprestamos.MdiParent = this;
            frmprestamos.Show();

        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formulario = Application.OpenForms["frmReportes"];

            if (formulario != null)
            {
                formulario.Activate();
                return;
            }

            frmReportes frmreportes = new frmReportes(this);
            frmreportes.MdiParent = this;
            frmreportes.Show();
        }

       
    }
}
