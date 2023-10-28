namespace wfaCopyData
{
    partial class Form1
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
            this.btnCopy = new System.Windows.Forms.Button();
            this.LBBaseDatos = new System.Windows.Forms.ListBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.LBTablas = new System.Windows.Forms.ListBox();
            this.LBCampos = new System.Windows.Forms.ListBox();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(67, 12);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(99, 49);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "Iniciar.";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // LBBaseDatos
            // 
            this.LBBaseDatos.ItemHeight = 25;
            this.LBBaseDatos.Location = new System.Drawing.Point(28, 65);
            this.LBBaseDatos.Margin = new System.Windows.Forms.Padding(6);
            this.LBBaseDatos.Name = "LBBaseDatos";
            this.LBBaseDatos.Size = new System.Drawing.Size(346, 329);
            this.LBBaseDatos.TabIndex = 28;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(1067, 24);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(99, 49);
            this.btnGenerar.TabIndex = 29;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // LBTablas
            // 
            this.LBTablas.ItemHeight = 25;
            this.LBTablas.Location = new System.Drawing.Point(559, 65);
            this.LBTablas.Margin = new System.Windows.Forms.Padding(6);
            this.LBTablas.Name = "LBTablas";
            this.LBTablas.Size = new System.Drawing.Size(346, 329);
            this.LBTablas.TabIndex = 30;
            // 
            // LBCampos
            // 
            this.LBCampos.ItemHeight = 25;
            this.LBCampos.Location = new System.Drawing.Point(971, 65);
            this.LBCampos.Margin = new System.Windows.Forms.Padding(6);
            this.LBCampos.Name = "LBCampos";
            this.LBCampos.Size = new System.Drawing.Size(346, 329);
            this.LBCampos.TabIndex = 31;
            // 
            // dataGrid1
            // 
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(28, 449);
            this.dataGrid1.Margin = new System.Windows.Forms.Padding(6);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.Size = new System.Drawing.Size(1289, 446);
            this.dataGrid1.TabIndex = 32;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(617, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 49);
            this.button1.TabIndex = 33;
            this.button1.Text = "Ver datos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSeleccionarBase_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1369, 983);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.LBCampos);
            this.Controls.Add(this.LBTablas);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.LBBaseDatos);
            this.Controls.Add(this.btnCopy);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ListBox LBBaseDatos;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.ListBox LBTablas;
        private System.Windows.Forms.ListBox LBCampos;
        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Button button1;
    }
}

