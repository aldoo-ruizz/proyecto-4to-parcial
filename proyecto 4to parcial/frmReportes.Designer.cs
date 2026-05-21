namespace proyecto_4to_parcial
{
    partial class frmReportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLibrosPrestados = new System.Windows.Forms.Button();
            this.btnSociosActivos = new System.Windows.Forms.Button();
            this.btnPrestamosActivos = new System.Windows.Forms.Button();
            this.btnSociossPrestamos = new System.Windows.Forms.Button();
            this.btnLibrosDisponibles = new System.Windows.Forms.Button();
            this.dgvReportes = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLibrosPrestados
            // 
            this.btnLibrosPrestados.Location = new System.Drawing.Point(38, 44);
            this.btnLibrosPrestados.Name = "btnLibrosPrestados";
            this.btnLibrosPrestados.Size = new System.Drawing.Size(225, 34);
            this.btnLibrosPrestados.TabIndex = 0;
            this.btnLibrosPrestados.Text = "Libros más prestados";
            this.btnLibrosPrestados.UseVisualStyleBackColor = true;
            // 
            // btnSociosActivos
            // 
            this.btnSociosActivos.Location = new System.Drawing.Point(38, 99);
            this.btnSociosActivos.Name = "btnSociosActivos";
            this.btnSociosActivos.Size = new System.Drawing.Size(225, 34);
            this.btnSociosActivos.TabIndex = 1;
            this.btnSociosActivos.Text = "Socios más activos";
            this.btnSociosActivos.UseVisualStyleBackColor = true;
            // 
            // btnPrestamosActivos
            // 
            this.btnPrestamosActivos.Location = new System.Drawing.Point(38, 158);
            this.btnPrestamosActivos.Name = "btnPrestamosActivos";
            this.btnPrestamosActivos.Size = new System.Drawing.Size(225, 34);
            this.btnPrestamosActivos.TabIndex = 2;
            this.btnPrestamosActivos.Text = "Prestamos activos";
            this.btnPrestamosActivos.UseVisualStyleBackColor = true;
            // 
            // btnSociossPrestamos
            // 
            this.btnSociossPrestamos.Location = new System.Drawing.Point(38, 277);
            this.btnSociossPrestamos.Name = "btnSociossPrestamos";
            this.btnSociossPrestamos.Size = new System.Drawing.Size(225, 34);
            this.btnSociossPrestamos.TabIndex = 4;
            this.btnSociossPrestamos.Text = "Socios sin prestamos";
            this.btnSociossPrestamos.UseVisualStyleBackColor = true;
            // 
            // btnLibrosDisponibles
            // 
            this.btnLibrosDisponibles.Location = new System.Drawing.Point(38, 218);
            this.btnLibrosDisponibles.Name = "btnLibrosDisponibles";
            this.btnLibrosDisponibles.Size = new System.Drawing.Size(225, 34);
            this.btnLibrosDisponibles.TabIndex = 3;
            this.btnLibrosDisponibles.Text = "Libros disponibles";
            this.btnLibrosDisponibles.UseVisualStyleBackColor = true;
            // 
            // dgvReportes
            // 
            this.dgvReportes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportes.Location = new System.Drawing.Point(295, 32);
            this.dgvReportes.Name = "dgvReportes";
            this.dgvReportes.RowHeadersWidth = 51;
            this.dgvReportes.RowTemplate.Height = 24;
            this.dgvReportes.Size = new System.Drawing.Size(450, 328);
            this.dgvReportes.TabIndex = 5;
            // 
            // frmReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 408);
            this.Controls.Add(this.dgvReportes);
            this.Controls.Add(this.btnSociossPrestamos);
            this.Controls.Add(this.btnLibrosDisponibles);
            this.Controls.Add(this.btnPrestamosActivos);
            this.Controls.Add(this.btnSociosActivos);
            this.Controls.Add(this.btnLibrosPrestados);
            this.Name = "frmReportes";
            this.Text = "frmReportes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLibrosPrestados;
        private System.Windows.Forms.Button btnSociosActivos;
        private System.Windows.Forms.Button btnPrestamosActivos;
        private System.Windows.Forms.Button btnSociossPrestamos;
        private System.Windows.Forms.Button btnLibrosDisponibles;
        private System.Windows.Forms.DataGridView dgvReportes;
    }
}