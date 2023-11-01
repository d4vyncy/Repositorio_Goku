namespace GeneradorCodigoControladoras
{
    partial class FormMysql
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMysql));
            this.Error2 = new System.Windows.Forms.Label();
            this.TPAcercaDeD4VYNCY = new System.Windows.Forms.TabControl();
            this.TPFicheros = new System.Windows.Forms.TabPage();
            this.Error1 = new System.Windows.Forms.Label();
            this.TPTablas = new System.Windows.Forms.TabPage();
            this.RTBCodigoGenerado = new System.Windows.Forms.RichTextBox();
            this.BTNCargarCampos = new System.Windows.Forms.Button();
            this.guardarArchivoTexto = new System.Windows.Forms.Button();
            this.LBCampos = new System.Windows.Forms.ListBox();
            this.BTNGenerarCodigo = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LBTablas = new System.Windows.Forms.ListBox();
            this.TPGenerarCodigoPasos = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TXTUsuario = new System.Windows.Forms.TextBox();
            this.TXTContrasena = new System.Windows.Forms.TextBox();
            this.BTNAutenticar = new System.Windows.Forms.Button();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.LBBaseDatos = new System.Windows.Forms.ListBox();
            this.BTNConectarBaseDeDatos = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.TXTSever = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.BTNConectarServidor = new System.Windows.Forms.Button();
            this.TXTPass = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.TXTUser = new System.Windows.Forms.TextBox();
            this.TXTWebservices = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.TXTRutaArchivo = new System.Windows.Forms.TextBox();
            this.CLBSeleccionCodigo = new System.Windows.Forms.CheckedListBox();
            this.BTGenerarAutomaticamente = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TPAcercaDeD4VYNCY.SuspendLayout();
            this.TPFicheros.SuspendLayout();
            this.TPTablas.SuspendLayout();
            this.TPGenerarCodigoPasos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Error2
            // 
            this.Error2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Error2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Error2.Location = new System.Drawing.Point(23, 67);
            this.Error2.Name = "Error2";
            this.Error2.Size = new System.Drawing.Size(635, 23);
            this.Error2.TabIndex = 23;
            this.Error2.Click += new System.EventHandler(this.Error2_Click);
            // 
            // TPAcercaDeD4VYNCY
            // 
            this.TPAcercaDeD4VYNCY.Controls.Add(this.TPFicheros);
            this.TPAcercaDeD4VYNCY.Controls.Add(this.TPTablas);
            this.TPAcercaDeD4VYNCY.Controls.Add(this.TPGenerarCodigoPasos);
            this.TPAcercaDeD4VYNCY.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TPAcercaDeD4VYNCY.Location = new System.Drawing.Point(18, 449);
            this.TPAcercaDeD4VYNCY.Name = "TPAcercaDeD4VYNCY";
            this.TPAcercaDeD4VYNCY.SelectedIndex = 0;
            this.TPAcercaDeD4VYNCY.Size = new System.Drawing.Size(640, 120);
            this.TPAcercaDeD4VYNCY.TabIndex = 22;
            // 
            // TPFicheros
            // 
            this.TPFicheros.Controls.Add(this.Error1);
            this.TPFicheros.Location = new System.Drawing.Point(4, 22);
            this.TPFicheros.Name = "TPFicheros";
            this.TPFicheros.Padding = new System.Windows.Forms.Padding(3);
            this.TPFicheros.Size = new System.Drawing.Size(632, 94);
            this.TPFicheros.TabIndex = 4;
            this.TPFicheros.Text = "Ficheros";
            this.TPFicheros.UseVisualStyleBackColor = true;
            // 
            // Error1
            // 
            this.Error1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Error1.ForeColor = System.Drawing.Color.Red;
            this.Error1.Location = new System.Drawing.Point(30, 250);
            this.Error1.Name = "Error1";
            this.Error1.Size = new System.Drawing.Size(583, 23);
            this.Error1.TabIndex = 34;
            // 
            // TPTablas
            // 
            this.TPTablas.Controls.Add(this.RTBCodigoGenerado);
            this.TPTablas.Controls.Add(this.BTNCargarCampos);
            this.TPTablas.Controls.Add(this.guardarArchivoTexto);
            this.TPTablas.Controls.Add(this.LBCampos);
            this.TPTablas.Controls.Add(this.BTNGenerarCodigo);
            this.TPTablas.Controls.Add(this.label4);
            this.TPTablas.Controls.Add(this.label3);
            this.TPTablas.Controls.Add(this.label5);
            this.TPTablas.Controls.Add(this.LBTablas);
            this.TPTablas.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TPTablas.Location = new System.Drawing.Point(4, 22);
            this.TPTablas.Name = "TPTablas";
            this.TPTablas.Padding = new System.Windows.Forms.Padding(3);
            this.TPTablas.Size = new System.Drawing.Size(632, 94);
            this.TPTablas.TabIndex = 1;
            this.TPTablas.Text = "Tablas";
            this.TPTablas.UseVisualStyleBackColor = true;
            // 
            // RTBCodigoGenerado
            // 
            this.RTBCodigoGenerado.Location = new System.Drawing.Point(312, 35);
            this.RTBCodigoGenerado.Name = "RTBCodigoGenerado";
            this.RTBCodigoGenerado.Size = new System.Drawing.Size(298, 212);
            this.RTBCodigoGenerado.TabIndex = 36;
            this.RTBCodigoGenerado.Text = "";
            // 
            // BTNCargarCampos
            // 
            this.BTNCargarCampos.Location = new System.Drawing.Point(162, 253);
            this.BTNCargarCampos.Name = "BTNCargarCampos";
            this.BTNCargarCampos.Size = new System.Drawing.Size(144, 23);
            this.BTNCargarCampos.TabIndex = 35;
            this.BTNCargarCampos.Text = "Cargar Campos";
            this.BTNCargarCampos.Click += new System.EventHandler(this.BTNCargarCampos_Click);
            // 
            // guardarArchivoTexto
            // 
            this.guardarArchivoTexto.Location = new System.Drawing.Point(535, 6);
            this.guardarArchivoTexto.Name = "guardarArchivoTexto";
            this.guardarArchivoTexto.Size = new System.Drawing.Size(75, 23);
            this.guardarArchivoTexto.TabIndex = 37;
            this.guardarArchivoTexto.Text = "Guardar como";
            this.guardarArchivoTexto.UseVisualStyleBackColor = true;
            this.guardarArchivoTexto.Click += new System.EventHandler(this.guardarArchivoTexto_Click);
            // 
            // LBCampos
            // 
            this.LBCampos.Location = new System.Drawing.Point(162, 35);
            this.LBCampos.Name = "LBCampos";
            this.LBCampos.Size = new System.Drawing.Size(144, 212);
            this.LBCampos.TabIndex = 34;
            // 
            // BTNGenerarCodigo
            // 
            this.BTNGenerarCodigo.Location = new System.Drawing.Point(312, 253);
            this.BTNGenerarCodigo.Name = "BTNGenerarCodigo";
            this.BTNGenerarCodigo.Size = new System.Drawing.Size(298, 23);
            this.BTNGenerarCodigo.TabIndex = 33;
            this.BTNGenerarCodigo.Text = "GenerarCodigo";
            this.BTNGenerarCodigo.Click += new System.EventHandler(this.CargarCampos_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(381, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "Controladora Generada";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(170, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 16);
            this.label3.TabIndex = 28;
            this.label3.Text = "Campos de la tabla";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(24, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tablas Existentes";
            // 
            // LBTablas
            // 
            this.LBTablas.Location = new System.Drawing.Point(18, 35);
            this.LBTablas.Name = "LBTablas";
            this.LBTablas.Size = new System.Drawing.Size(138, 238);
            this.LBTablas.TabIndex = 8;
            // 
            // TPGenerarCodigoPasos
            // 
            this.TPGenerarCodigoPasos.Controls.Add(this.pictureBox1);
            this.TPGenerarCodigoPasos.Controls.Add(this.label2);
            this.TPGenerarCodigoPasos.Controls.Add(this.label1);
            this.TPGenerarCodigoPasos.Controls.Add(this.TXTUsuario);
            this.TPGenerarCodigoPasos.Controls.Add(this.TXTContrasena);
            this.TPGenerarCodigoPasos.Controls.Add(this.BTNAutenticar);
            this.TPGenerarCodigoPasos.Location = new System.Drawing.Point(4, 22);
            this.TPGenerarCodigoPasos.Name = "TPGenerarCodigoPasos";
            this.TPGenerarCodigoPasos.Padding = new System.Windows.Forms.Padding(3);
            this.TPGenerarCodigoPasos.Size = new System.Drawing.Size(632, 94);
            this.TPGenerarCodigoPasos.TabIndex = 8;
            this.TPGenerarCodigoPasos.Text = "Acerca de Autenticarse :)";
            this.TPGenerarCodigoPasos.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(340, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Usuarrio";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Contraseña";
            // 
            // TXTUsuario
            // 
            this.TXTUsuario.Enabled = false;
            this.TXTUsuario.Location = new System.Drawing.Point(135, 104);
            this.TXTUsuario.Name = "TXTUsuario";
            this.TXTUsuario.PasswordChar = '1';
            this.TXTUsuario.Size = new System.Drawing.Size(181, 20);
            this.TXTUsuario.TabIndex = 22;
            // 
            // TXTContrasena
            // 
            this.TXTContrasena.Enabled = false;
            this.TXTContrasena.Location = new System.Drawing.Point(135, 132);
            this.TXTContrasena.Name = "TXTContrasena";
            this.TXTContrasena.PasswordChar = 'e';
            this.TXTContrasena.Size = new System.Drawing.Size(181, 20);
            this.TXTContrasena.TabIndex = 21;
            // 
            // BTNAutenticar
            // 
            this.BTNAutenticar.Enabled = false;
            this.BTNAutenticar.Location = new System.Drawing.Point(135, 158);
            this.BTNAutenticar.Name = "BTNAutenticar";
            this.BTNAutenticar.Size = new System.Drawing.Size(181, 24);
            this.BTNAutenticar.TabIndex = 20;
            this.BTNAutenticar.Text = "&Autenticar";
            this.BTNAutenticar.Click += new System.EventHandler(this.BTNAutenticar_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(433, 48);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.Size = new System.Drawing.Size(171, 160);
            this.dataGrid1.TabIndex = 28;
            // 
            // LBBaseDatos
            // 
            this.LBBaseDatos.Location = new System.Drawing.Point(233, 48);
            this.LBBaseDatos.Name = "LBBaseDatos";
            this.LBBaseDatos.Size = new System.Drawing.Size(175, 160);
            this.LBBaseDatos.TabIndex = 27;
            // 
            // BTNConectarBaseDeDatos
            // 
            this.BTNConectarBaseDeDatos.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BTNConectarBaseDeDatos.Location = new System.Drawing.Point(130, 33);
            this.BTNConectarBaseDeDatos.Name = "BTNConectarBaseDeDatos";
            this.BTNConectarBaseDeDatos.Size = new System.Drawing.Size(64, 23);
            this.BTNConectarBaseDeDatos.TabIndex = 26;
            this.BTNConectarBaseDeDatos.Text = "&Conectar Base de Datos";
            this.BTNConectarBaseDeDatos.UseVisualStyleBackColor = true;
            this.BTNConectarBaseDeDatos.Visible = false;
            this.BTNConectarBaseDeDatos.Click += new System.EventHandler(this.BTNConectarBaseDeDatos_Click);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(230, 29);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(232, 16);
            this.label19.TabIndex = 24;
            this.label19.Text = "Vista previa de los campos seleccionados";
            // 
            // TXTSever
            // 
            this.TXTSever.Location = new System.Drawing.Point(26, 62);
            this.TXTSever.Name = "TXTSever";
            this.TXTSever.Size = new System.Drawing.Size(184, 20);
            this.TXTSever.TabIndex = 15;
            this.TXTSever.Text = "195.35.38.67";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(23, 29);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(100, 16);
            this.label18.TabIndex = 21;
            this.label18.Text = "Nombre Servidor:";
            // 
            // BTNConectarServidor
            // 
            this.BTNConectarServidor.Location = new System.Drawing.Point(26, 214);
            this.BTNConectarServidor.Name = "BTNConectarServidor";
            this.BTNConectarServidor.Size = new System.Drawing.Size(184, 23);
            this.BTNConectarServidor.TabIndex = 20;
            this.BTNConectarServidor.Text = "Coneccion Servidor";
            this.BTNConectarServidor.UseVisualStyleBackColor = true;
            this.BTNConectarServidor.Click += new System.EventHandler(this.BTNConectarServidor_Click);
            // 
            // TXTPass
            // 
            this.TXTPass.Location = new System.Drawing.Point(28, 181);
            this.TXTPass.Name = "TXTPass";
            this.TXTPass.PasswordChar = '*';
            this.TXTPass.Size = new System.Drawing.Size(184, 20);
            this.TXTPass.TabIndex = 17;
            this.TXTPass.Text = "J*v7DbKmhCT#hxM";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(25, 157);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 16);
            this.label16.TabIndex = 19;
            this.label16.Text = "Contraseña";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(25, 92);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 16);
            this.label17.TabIndex = 18;
            this.label17.Text = "Cuenta";
            // 
            // TXTUser
            // 
            this.TXTUser.Location = new System.Drawing.Point(28, 119);
            this.TXTUser.Name = "TXTUser";
            this.TXTUser.Size = new System.Drawing.Size(184, 20);
            this.TXTUser.TabIndex = 16;
            this.TXTUser.Text = "u765344825_davidserrudo";
            // 
            // TXTWebservices
            // 
            this.TXTWebservices.Location = new System.Drawing.Point(26, 35);
            this.TXTWebservices.Name = "TXTWebservices";
            this.TXTWebservices.Size = new System.Drawing.Size(115, 20);
            this.TXTWebservices.TabIndex = 36;
            this.TXTWebservices.Text = "u765344825_sistemaclinica";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(23, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 16);
            this.label6.TabIndex = 36;
            this.label6.Text = "Nombre de la Aplicacion";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.HelpRequest += new System.EventHandler(this.guardarArchivoTexto_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(156, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(258, 19);
            this.label7.TabIndex = 39;
            this.label7.Text = "Ingrese la ruta donde se va a guardar el archivo";
            // 
            // TXTRutaArchivo
            // 
            this.TXTRutaArchivo.Location = new System.Drawing.Point(159, 35);
            this.TXTRutaArchivo.Name = "TXTRutaArchivo";
            this.TXTRutaArchivo.ReadOnly = true;
            this.TXTRutaArchivo.Size = new System.Drawing.Size(255, 20);
            this.TXTRutaArchivo.TabIndex = 38;
            this.TXTRutaArchivo.Text = "C:\\gen\\";
            // 
            // CLBSeleccionCodigo
            // 
            this.CLBSeleccionCodigo.FormattingEnabled = true;
            this.CLBSeleccionCodigo.Items.AddRange(new object[] {
            "Entidades",
            "Controladoras",
            "WebMetodos"});
            this.CLBSeleccionCodigo.Location = new System.Drawing.Point(538, 6);
            this.CLBSeleccionCodigo.Name = "CLBSeleccionCodigo";
            this.CLBSeleccionCodigo.Size = new System.Drawing.Size(120, 4);
            this.CLBSeleccionCodigo.TabIndex = 40;
            // 
            // BTGenerarAutomaticamente
            // 
            this.BTGenerarAutomaticamente.Location = new System.Drawing.Point(233, 214);
            this.BTGenerarAutomaticamente.Name = "BTGenerarAutomaticamente";
            this.BTGenerarAutomaticamente.Size = new System.Drawing.Size(370, 23);
            this.BTGenerarAutomaticamente.TabIndex = 35;
            this.BTGenerarAutomaticamente.Text = "&Generar Automaticamente la Capa de Negocio";
            this.BTGenerarAutomaticamente.UseVisualStyleBackColor = true;
            this.BTGenerarAutomaticamente.Click += new System.EventHandler(this.BTGenerarAutomaticamente_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(420, 32);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(87, 23);
            this.btnBuscar.TabIndex = 35;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.dataGrid1);
            this.groupBox1.Controls.Add(this.BTGenerarAutomaticamente);
            this.groupBox1.Controls.Add(this.TXTUser);
            this.groupBox1.Controls.Add(this.LBBaseDatos);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.BTNConectarBaseDeDatos);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.TXTPass);
            this.groupBox1.Controls.Add(this.TXTSever);
            this.groupBox1.Controls.Add(this.BTNConectarServidor);
            this.groupBox1.Location = new System.Drawing.Point(29, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(629, 244);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Generar capa de Negocio";
            // 
            // FormMysql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 431);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.CLBSeleccionCodigo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TXTRutaArchivo);
            this.Controls.Add(this.Error2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TXTWebservices);
            this.Controls.Add(this.TPAcercaDeD4VYNCY);
            this.MaximumSize = new System.Drawing.Size(672, 486);
            this.MinimumSize = new System.Drawing.Size(672, 400);
            this.Name = "FormMysql";
            this.Text = "Generador de codigo estandas para la capa de negocio \"davidserrudo@gmail.com\"";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TPAcercaDeD4VYNCY.ResumeLayout(false);
            this.TPFicheros.ResumeLayout(false);
            this.TPTablas.ResumeLayout(false);
            this.TPGenerarCodigoPasos.ResumeLayout(false);
            this.TPGenerarCodigoPasos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Error2;
        private System.Windows.Forms.TabControl TPAcercaDeD4VYNCY;
        private System.Windows.Forms.TabPage TPFicheros;
        private System.Windows.Forms.Label Error1;
        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.ListBox LBBaseDatos;
        private System.Windows.Forms.Button BTNConectarBaseDeDatos;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox TXTSever;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button BTNConectarServidor;
        private System.Windows.Forms.TextBox TXTPass;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox TXTUser;
        private System.Windows.Forms.TabPage TPTablas;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LBTablas;
        private System.Windows.Forms.TabPage TPGenerarCodigoPasos;
        private System.Windows.Forms.Button BTNAutenticar;
        private System.Windows.Forms.Button BTNGenerarCodigo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TXTUsuario;
        private System.Windows.Forms.TextBox TXTContrasena;
        private System.Windows.Forms.Button BTNCargarCampos;
        private System.Windows.Forms.ListBox LBCampos;
        private System.Windows.Forms.TextBox TXTWebservices;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button guardarArchivoTexto;
        private System.Windows.Forms.RichTextBox RTBCodigoGenerado;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TXTRutaArchivo;
        private System.Windows.Forms.CheckedListBox CLBSeleccionCodigo;
        private System.Windows.Forms.Button BTGenerarAutomaticamente;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

