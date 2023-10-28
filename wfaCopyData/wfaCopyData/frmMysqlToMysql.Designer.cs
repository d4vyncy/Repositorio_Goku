namespace wfaCopyData
{
    partial class frmMysqlToMysql
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
            this.LBTablas = new System.Windows.Forms.ListBox();
            this.btnConectar = new System.Windows.Forms.Button();
            this.LBCampos = new System.Windows.Forms.ListBox();
            this.btnExportar = new System.Windows.Forms.Button();
            this.LBColumnas = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // LBTablas
            // 
            this.LBTablas.ItemHeight = 16;
            this.LBTablas.Location = new System.Drawing.Point(45, 67);
            this.LBTablas.Margin = new System.Windows.Forms.Padding(4);
            this.LBTablas.Name = "LBTablas";
            this.LBTablas.Size = new System.Drawing.Size(232, 212);
            this.LBTablas.TabIndex = 33;
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(90, 11);
            this.btnConectar.Margin = new System.Windows.Forms.Padding(2);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(66, 31);
            this.btnConectar.TabIndex = 32;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // LBCampos
            // 
            this.LBCampos.ItemHeight = 16;
            this.LBCampos.Location = new System.Drawing.Point(913, 79);
            this.LBCampos.Margin = new System.Windows.Forms.Padding(4);
            this.LBCampos.Name = "LBCampos";
            this.LBCampos.Size = new System.Drawing.Size(232, 212);
            this.LBCampos.TabIndex = 35;
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(977, 29);
            this.btnExportar.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(66, 31);
            this.btnExportar.TabIndex = 34;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // LBColumnas
            // 
            this.LBColumnas.ItemHeight = 16;
            this.LBColumnas.Location = new System.Drawing.Point(434, 67);
            this.LBColumnas.Margin = new System.Windows.Forms.Padding(4);
            this.LBColumnas.Name = "LBColumnas";
            this.LBColumnas.Size = new System.Drawing.Size(232, 212);
            this.LBColumnas.TabIndex = 36;
            // 
            // frmMysqlToMysql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 735);
            this.Controls.Add(this.LBColumnas);
            this.Controls.Add(this.LBCampos);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.LBTablas);
            this.Controls.Add(this.btnConectar);
            this.Name = "frmMysqlToMysql";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmMysqlToMysql";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LBTablas;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.ListBox LBCampos;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.ListBox LBColumnas;
    }
}
