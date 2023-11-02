using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace GeneradorCodigoControladoras
{
    public partial class FormMysql : Form
    {
        public Entidades.EBaseDatos BaseDatos = new GeneradorCodigoControladoras.Entidades.EBaseDatos();
        public Entidades.EImportarSQL ImportarSQL = new GeneradorCodigoControladoras.Entidades.EImportarSQL();
        public Entidades.EOrdenar eOrdenar = new GeneradorCodigoControladoras.Entidades.EOrdenar();
        public DataSet dts = new DataSet();
        public string CadenaConexion;
        
        public FormMysql()
        {
            InitializeComponent();
            if (DateTime.Now > Convert.ToDateTime("01/01/2024"))
                Dispose();
            //BTNConectarBaseDeDatos.Enabled = false; BTNGenerarCodigo.Enabled = false;
        }

        private void BTNConectarServidor_Click(object sender, EventArgs e)
        {
            Error1.Text = "Conectando a la Base de datos...";
            Error1.ForeColor = Color.Blue;
            //string CadenaConexion;
            BaseDatos._Cuenta = TXTUser.Text;
            //BaseDatos._NombreBaseDatos = TXTBaseDatos1.Text;
            BaseDatos._NombreBaseDatos = TXTWebservices.Text;
            BaseDatos._NombreServidor = TXTSever.Text;
            BaseDatos._Password = TXTPass.Text;
            Error2.Text = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            //BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            BaseDatos.ConnectionString = "Server=" + BaseDatos.NombreServidor + ";Database=" + BaseDatos.NombreBaseDatos + ";UID=" + BaseDatos.Cuenta + ";Password =" + BaseDatos.Password + ";CHARSET=utf8;convert zero datetime=True";
            try
            {
                DataSet dsResultados = new DataSet();
                MySqlConnection MiConexion = new MySqlConnection(BaseDatos.ConnectionString);
                MySqlDataAdapter MiDataAdapter = new MySqlDataAdapter();
                MiConexion.Open();
                //string MySql = "sp_databases";
                string MySql = "SELECT SCHEMA_NAME 'Database' FROM information_schema.schemata WHERE  schema_name not LIKE 'information_schema%';";
                MySqlCommand MiCommand = new MySqlCommand(MySql, MiConexion);
                MiCommand.CommandType = CommandType.Text;
                MiDataAdapter.SelectCommand = MiCommand;
                DataSet ds = new DataSet("Tabla");
                MiDataAdapter.Fill(ds);
                MiConexion.Close();

                LBBaseDatos.DataSource = ds.Tables["Table"];
                LBBaseDatos.DisplayMember = "Database";
                LBBaseDatos.ValueMember = "Database";
                LBBaseDatos.Refresh();
                //--//
                Error1.Text = "Conexion Exitosa... Puede continuar";
                Error1.ForeColor = Color.Blue;
                BaseDatos._Conectado = true;
            }
            catch
            {
                Error1.Text = "La conexión ha fallado...";
                Error1.ForeColor = Color.Red;
                BaseDatos.Conectado = false;
            }
        }

        private void BTNConectarBaseDeDatos_Click(object sender, EventArgs e)
        {
            BaseDatos._NombreBaseDatos = LBBaseDatos.SelectedValue.ToString();
            //MessageBox.Show(LBBaseDatos.SelectedValue.ToString());
            BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            TXTWebservices.Text = BaseDatos.NombreBaseDatos;
            Error2.Text = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            #region cargar el listobox con los valores de la tabla
            LBTablas.Items.Clear();
            LBTablas.Refresh();
            try
            {
                DataSet ds = new DataSet("Tabla");
                ds = ImportarSQL.TablasSqlServer(BaseDatos.ConnectionString, "");
                dataGrid1.DataSource = ds.Tables["table"];
                dataGrid1.Refresh();
                LBTablas.DataSource = ds.Tables["table"];
                LBTablas.DisplayMember = "TABLE_NAME";
                LBTablas.ValueMember = "TABLE_NAME";
                LBTablas.Refresh();
                Error1.Text = "Conexion Exitosa... Puede continuar";
                Error1.ForeColor = Color.Blue;
                BaseDatos._Conectado = true;
                //--//

            }
            catch
            {
                Error1.Text = "La conexión ha fallado...";
                Error1.ForeColor = Color.Red;
                BaseDatos.Conectado = false;
            }
            #endregion
        }

        private void CargarCampos_Click(object sender, EventArgs e)
        {
            RTBCodigoGenerado.Text = "//CODIGO D4VYNCY CONSULTAR CORREO D4VYNCY@HOTMAIL.COM";
            BTNGenerarCodigo.Enabled = false;
            string codBusqueda;
            string comillasIgual = @""" = '""".ToString() + @"""".Substring(1);
            string comillasSeguidas = @"""'""";
            string like1 = @""" like '*""";
            string por = @"""*'""";
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "Entidades")
            {
                //creamos las entidades de los web services
                CrearLasEntidades();
            }
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "Controladoras")
            {
                //creamos las controladoras del web services
                CrearLasControladoras();
            }

            RTBCodigoGenerado.Focus();
            RTBCodigoGenerado.SelectAll();
            //saveFileDialog1.FileName.Replace("nose.txt", "uno.txt");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TXTUsuario.Enabled = true;
            TXTContrasena.Enabled = true;
            BTNAutenticar.Enabled = true;
        }

        private void BTNAutenticar_Click(object sender, EventArgs e)
        {
            string use;
            string pass;
            int num, num1;
            num = Convert.ToInt32(DateTime.Today.Day.ToString());
            num = num * Convert.ToInt32(DateTime.Today.Month.ToString());
            num1 = Convert.ToInt32(DateTime.Today.Year.ToString());
            if ((num % 2) == 0)
            {
                num1 += num;
            }
            else
            {
                num1 -= num;
            }
            use = num.ToString();
            pass = num1.ToString();
            if (TXTContrasena.Text == pass && TXTUsuario.Text == use)
            {
                BTNConectarBaseDeDatos.Enabled = true;
            }
        }

        private void BTNCargarCampos_Click(object sender, EventArgs e)
        {
            //BTNGenerarCodigo.Enabled = true;
            //BaseDatos.Tablas.Clear();
            //BaseDatos.Tablas.Add(LBTablas.SelectedValue);
            //string conect = Error2.Text;
            //this.ImportarSQL.ColumnasI.Clear();
            //this.ImportarSQL.TypoColumnasI.Clear();            
            //this.ImportarSQL.CargarColumnas(conect, BaseDatos.Tablas[0].ToString());
            //this.LBCampos.Items.Clear();            
            //for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            //{

            //        this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
            //        this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);


            //}
        }



        private void Error2_Click(object sender, EventArgs e)
        {

            try
            {
                DataSet ds = new DataSet("Tabla");
                ds = ImportarSQL.TablasSqlServer(Error2.Text.ToString(), ParametroAcronimo);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MessageBox.Show(ds.Tables[0].Columns[0].Table.Rows[i].ToString());
                }
            }
            catch
            {
                Error1.Text = "falla al momento de cargar los datos";
            }
        }
        #region validacion de datos
        public string validarDato(string dato)
        {
            if (dato == "text")
            { dato = "string"; }
            if (dato == "varchar")
            { dato = "string"; }
            if (dato == "numeric")
            { dato = "decimal"; }
            if (dato == "Int32")
            { dato = "int"; }
            if (dato == "Decimal")
            { dato = "decimal"; }
            if (dato == "Char")
            { dato = "string"; }
            if (dato == "String")
            { dato = "string"; }
            if (dato == "Int16")
            { dato = "int"; }
            if (dato == "Double")
            { dato = "double"; }
            return dato;
        }
        public string validarDatoMysql(string dato)
        {
            if (dato == "text")
            { dato = "string"; }
            if (dato == "varchar")
            { dato = "string"; }
            if (dato == "numeric")
            { dato = "number"; }
            if (dato == "Int32")
            { dato = "number"; }
            if (dato == "Decimal")
            { dato = "number"; }
            if (dato == "Char")
            { dato = "string"; }
            if (dato == "String")
            { dato = "string"; }
            if (dato == "Int16")
            { dato = "number"; }
            if (dato == "Double")
            { dato = "number"; }
            if (dato == "Byte[]")
            { dato = "string"; }
            return dato;
        }
        public string validarDatoMysqlValor(string dato)
        {
            if (dato == "text")
            { dato = "''"; }
            if (dato == "varchar")
            { dato = "''"; }
            if (dato == "numeric")
            { dato = "0"; }
            if (dato == "Int32")
            { dato = "0"; }
            if (dato == "Decimal")
            { dato = "0"; }
            if (dato == "Char")
            { dato = "''"; }
            if (dato == "String")
            { dato = "''"; }
            if (dato == "Int16")
            { dato = "0"; }
            if (dato == "Double")
            { dato = "0"; }
            if (dato == "Byte[]")
            { dato = "''"; }
            if (dato == "Boolean")
            { dato = "false"; }
            return dato;
        }
        public string valorInicialDato(string dato)
        {
            if (dato == "text")
            { dato = "string.Empty"; }
            if (dato == "varchar")
            { dato = "string.Empty"; }
            if (dato == "numeric")
            { dato = "0"; }
            if (dato == "Int32")
            { dato = "0"; }
            if (dato == "Decimal")
            { dato = "0"; }
            if (dato == "Char")
            { dato = "string.Empty"; }
            if (dato == "string")
            { dato = "string.Empty"; }
            if (dato == "String")
            { dato = "string.Empty"; }
            if (dato == "Boolean")
            { dato = "true"; }
            if (dato == "DateTime")
            { dato = "DateTime.Now"; }
            if (dato == "Int16")
            { dato = "0"; }
            if (dato == "Int64")
            { dato = "0"; }
            if (dato == "Double")
            { dato = "0"; }

            return dato;
        }
        public string datoSQL(string dato)
        {
            if (dato == "text")
            { dato = "Text"; }
            if (dato == "varchar")
            { dato = "NVarChar,50"; }
            if (dato == "numeric")
            { dato = "Decimal,0"; }
            if (dato == "Int32")
            { dato = "Int,0"; }
            if (dato == "Int16")
            { dato = "Int,0"; }
            if (dato == "Decimal")
            { dato = "Decimal,0"; }
            if (dato == "Char")
            { dato = "Char"; }
            if (dato == "String")
            { dato = "NVarChar,50"; }
            if (dato == "Text")
            { dato = "Text"; }
            if (dato == "DateTime")
            { dato = "DateTime,0"; }
            if (dato == "Boolean")
            { dato = "Bit"; }
            if (dato == "Double")
            { dato = "Float"; }

            return dato;
        }
        #region tipo de data row anterior
        //public string TipoDeDatoDataRow(string dato, string NombreCampo, string NombreTabla)
        //{
        //    if (dato == "text")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "].ToString();"; }
        //    if (dato == "varchar")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "].ToString();"; }
        //    if (dato == "numeric")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDecimal(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Int32")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToInt32(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Int16")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToInt16(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Decimal")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDecimal(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Char")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "String")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Text")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "DateTime")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDateTime(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Boolean")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToBoolean(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }

        //    if (dato == "Double")
        //    { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDouble(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }


        //    return dato;
        //}
        #endregion
        public string TipoDeDatoDataRow(string dato, string NombreCampo, string NombreTabla)
        {
            if (dato == "text")
            { dato = "item." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "varchar")
            { dato = "item." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "numeric")
            { dato = "item." + NombreCampo + " = Convert.ToDecimal(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Int32")
            { dato = "item." + NombreCampo + " = Convert.ToInt32(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Int16")
            { dato = "item." + NombreCampo + " = Convert.ToInt16(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Decimal")
            { dato = "item." + NombreCampo + " = Convert.ToDecimal(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Char")
            { dato = "item." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "String")
            { dato = "item." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Text")
            { dato = "item." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "DateTime")
            { dato = "item." + NombreCampo + " = Convert.ToDateTime(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Boolean")
            { dato = "item." + NombreCampo + " = Convert.ToBoolean(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Double")
            { dato = "item." + NombreCampo + " = Convert.ToDouble(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Byte[]")
            { dato = "item." + NombreCampo + " = (byte[])dr[" + comi + NombreCampo + comi + "];"; }

            if (dato == "Byte")
            { dato = "item." + NombreCampo + " = Convert.ToByte(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Int64")
            { dato = "item." + NombreCampo + " = Convert.ToInt64(dr[" + comi + NombreCampo + comi + "]);"; }

            return dato;
        }
        public string TipoDeDatoDataRowParaLaData(string dato, string NombreCampo, string NombreTabla)
        {
            if (dato == "text")
            { dato = "this." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "varchar")
            { dato = "this." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "numeric")
            { dato = "this." + NombreCampo + " = Convert.ToDecimal(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Int32")
            { dato = "this." + NombreCampo + " = Convert.ToInt32(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Int16")
            { dato = "this." + NombreCampo + " = Convert.ToInt16(dr[" + comi + NombreCampo + comi + "]);"; }
            if (dato == "Decimal")
            { dato = "this." + NombreCampo + " = Convert.ToDecimal(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Char")
            { dato = "this." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "String")
            { dato = "this." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Text")
            { dato = "this." + NombreCampo + " = Convert.ToString(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "DateTime")
            { dato = "this." + NombreCampo + " = Convert.ToDateTime(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Boolean")
            { dato = "this." + NombreCampo + " = Convert.ToBoolean(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Double")
            { dato = "this." + NombreCampo + " = Convert.ToDouble(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Byte[]")
            { dato = "this." + NombreCampo + " = (byte[])dr[" + comi + NombreCampo + comi + "];"; }

            if (dato == "Byte")
            { dato = "this." + NombreCampo + " = Convert.ToByte(dr[" + comi + NombreCampo + comi + "]);"; }

            if (dato == "Int64")
            { dato = "this." + NombreCampo + " = Convert.ToInt64(dr[" + comi + NombreCampo + comi + "]);"; }


            return dato;
        }
        #endregion
        public void CrearLasEntidades()
        {
            string ComillaSimple, tipoDato, tipoSql;
            string commilla = @"""";
            string codBusqueda;
            string comillasIgual = @""" = '""".ToString() + @"""".Substring(1);
            string comillasSeguidas = @"""'""";
            string like1 = @""" like '*""";
            //" + TXTSever.Text.ToString() + "
            string por = @"""*'""";
            ComillaSimple = @"""";
            tipoDato = "";
            tipoSql = "";
            //parte de los using
            string NombreTabla = "";
            //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(5)));
            //else
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(6)));
            acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());

            RTBCodigoGenerado.Text += "export class " + NombreTabla + "Model" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{";

            //LLenamos los campos atributos de la entidad
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                //tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                tipoDato = validarDatoMysql(this.ImportarSQL.TypoColumnasI[i].ToString());
                if (this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                    RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i] + ":Date = new Date();";
                else
                    RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i] + " : " + tipoDato + ';' + Environment.NewLine;
            }

            RTBCodigoGenerado.Text += "constructor() {" + Environment.NewLine;

            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                //tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                if (!this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                {
                    tipoDato = validarDatoMysqlValor(this.ImportarSQL.TypoColumnasI[i].ToString());
                    RTBCodigoGenerado.Text += "this." + this.ImportarSQL.ColumnasI[i] + " = " + tipoDato + ';' + Environment.NewLine;
                }
            }
            RTBCodigoGenerado.Text += "}";

            RTBCodigoGenerado.Text += "}";

        }
        string comi = @"""";
        public void CrearLasControladoras()
        {
            string ComillaSimple, tipoDato, tipoSql;
            string commilla = @"""";
            string codBusqueda;
            string comillasIgual = @""" = '""".ToString() + @"""".Substring(1);
            string comillasSeguidas = @"""'""";
            string like1 = @""" like '*""";
            //" + TXTSever.Text.ToString() + "
            string por = @"""*'""";
            ComillaSimple = @"""";
            tipoDato = "";
            tipoSql = "";
            //parte de los using
            string NombreTabla = "";
            string NombreTablaSQL = "";
            //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(5)));
            //else
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(6)));
            acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());
            NombreTablaSQL = LBTablas.SelectedValue.ToString();
            RTBCodigoGenerado.Text += "<?php" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//gen " + DateTime.Now + " dst" + Environment.NewLine;
            RTBCodigoGenerado.Text += "class " + LBTablas.SelectedValue.ToString() + "_Ctrl" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "public $M_" + NombreTabla + " = null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public $M_Usuariologueado = null;";
            RTBCodigoGenerado.Text += "public function __construct()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$this->M_Usuariologueado = new M_Usuariologueado();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            #region para llenar los datos
            //selecionamos registros
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function sel" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$idusuario = is_null($f3->get('POST.idusuario')) ? 'T' : $f3->get('POST.idusuario');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$llave = is_null($f3->get('POST.llave')) ? 'T' : $f3->get('POST.llave');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave,'sel" + NombreTabla+ "',$f3)) {" + Environment.NewLine;
            //
            RTBCodigoGenerado.Text += "$pCampo0 = is_null($f3->get('POST.pCampo0')) ? 'T' : $f3->get('POST.pCampo0');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor0 = is_null($f3->get('POST.pValor0')) ? '' : $f3->get('POST.pValor0');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo1 = is_null($f3->get('POST.pCampo1')) ? 'T' : $f3->get('POST.pCampo1');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor1 = is_null($f3->get('POST.pValor1')) ? '' : $f3->get('POST.pValor1');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo2 = is_null($f3->get('POST.pCampo2')) ? 'T' : $f3->get('POST.pCampo2');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor2 = is_null($f3->get('POST.pValor2')) ? '' : $f3->get('POST.pValor2');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo3 = is_null($f3->get('POST.pCampo3')) ? 'T' : $f3->get('POST.pCampo3');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor3 = is_null($f3->get('POST.pValor3')) ? '' : $f3->get('POST.pValor3');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo4 = is_null($f3->get('POST.pCampo4')) ? 'T' : $f3->get('POST.pCampo4');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor4 = is_null($f3->get('POST.pValor4')) ? '' : $f3->get('POST.pValor4');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo5 = is_null($f3->get('POST.pCampo5')) ? 'T' : $f3->get('POST.pCampo5');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor5 = is_null($f3->get('POST.pValor5')) ? '' : $f3->get('POST.pValor5');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo6 = is_null($f3->get('POST.pCampo6')) ? 'T' : $f3->get('POST.pCampo6');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor6 = is_null($f3->get('POST.pValor6')) ? '' : $f3->get('POST.pValor6');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pCampo7 = is_null($f3->get('POST.pCampo7')) ? 'T' : $f3->get('POST.pCampo7');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$pValor7 = is_null($f3->get('POST.pValor7')) ? '' : $f3->get('POST.pValor7');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$oDav_Ctrol = new _Dav_Ctrol();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$Con = $oDav_Ctrol->fnDevuelveConsulta('" + NombreTablaSQL.Substring(0,4)+"vis"+ NombreTabla + "' , $pCampo0  , $pValor0  , $pCampo1  , $pValor1  , $pCampo2  , $pValor2  , $pCampo3  , $pValor3  , $pCampo4  , $pValor4  , $pCampo5  , $pValor5  , $pCampo6  , $pValor6  , $pCampo7  , $pValor7 );" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$sql = $Con;" + Environment.NewLine;

            //RTBCodigoGenerado.Text += "$sql = " + commilla + "CALL pvi" + NombreTabla + "('" + commilla + " . $pCampo0 . " + commilla + "','" + commilla + ". $pValor0 . " + commilla + "','" + commilla + ". $pCampo1 . " + commilla + "','" + commilla + ". $pValor1 . " + commilla + "','" + commilla + ". $pCampo2 . " + commilla + "','" + commilla + ". $pValor2 . " + commilla + "','" + commilla + ". $pCampo3 . " + commilla + "','" + commilla + ". $pValor3 . " + commilla + "','" + commilla + ". $pCampo4 . " + commilla + "','" + commilla + ". $pValor4 . " + commilla + "','" + commilla + ". $pCampo5 . " + commilla + "','" + commilla + ". $pValor5 . " + commilla + "','" + commilla + ". $pCampo6 . " + commilla + "','" + commilla + ". $pValor6 . " + commilla + "','" + commilla + ". $pCampo7 . " + commilla + "','" + commilla + ". $pValor7 . " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "$msg = 'Lista de " + NombreTabla + "s';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '200';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '500';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //por else
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += " $msg = '201';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //cerramos llaver
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //creamos los metodos CRUD
            //create
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function add" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$idusuario = is_null($f3->get('POST.idusuario')) ? 'T' : $f3->get('POST.idusuario');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$llave = is_null($f3->get('POST.llave')) ? 'T' : $f3->get('POST.llave');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave,'add" + NombreTabla + "',$f3)) {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave)) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$Tipo = 'C';" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$" + this.ImportarSQL.ColumnasI[i] + " = is_null($f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "')) ? 'T' : $f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "');" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$sql = " + commilla + "CALL "+ NombreTablaSQL.Substring(0,4)+"pag" + NombreTabla + "('" + commilla + " . $Tipo . " + commilla + "','" + commilla;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += ". $" + this.ImportarSQL.ColumnasI[i];
                if (i != (this.ImportarSQL.ColumnasI.Count - 1))
                    RTBCodigoGenerado.Text += " . " + commilla + "','" + commilla;
            }
            RTBCodigoGenerado.Text += ". " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '200';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '500';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //por else
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += " $msg = '201';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //read
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function get" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$idusuario = is_null($f3->get('POST.idusuario')) ? 'T' : $f3->get('POST.idusuario');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$llave = is_null($f3->get('POST.llave')) ? 'T' : $f3->get('POST.llave');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave,'get" + NombreTabla + "',$f3)) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$Tipo = 'R';" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$" + this.ImportarSQL.ColumnasI[i] + " = is_null($f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "')) ? 'T' : $f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "');" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$sql = " + commilla + "CALL " +NombreTablaSQL.Substring(0, 4) + "pag" + NombreTabla + "('" + commilla + " . $Tipo . " + commilla + "','" + commilla;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += ". $" + this.ImportarSQL.ColumnasI[i];
                if (i != (this.ImportarSQL.ColumnasI.Count - 1))
                    RTBCodigoGenerado.Text += " . " + commilla + "','" + commilla;
            }
            RTBCodigoGenerado.Text += ". " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '200';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '500';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //por else
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += " $msg = '201';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //update
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function upd" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$idusuario = is_null($f3->get('POST.idusuario')) ? 'T' : $f3->get('POST.idusuario');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$llave = is_null($f3->get('POST.llave')) ? 'T' : $f3->get('POST.llave');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave,'upd" + NombreTabla + "',$f3)) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$Tipo = 'U';" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$" + this.ImportarSQL.ColumnasI[i] + " = is_null($f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "')) ? 'T' : $f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "');" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$sql = " + commilla + "CALL "+ NombreTablaSQL.Substring(0, 4) + "pag" + NombreTabla + "('" + commilla + " . $Tipo . " + commilla + "','" + commilla;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += ". $" + this.ImportarSQL.ColumnasI[i];
                if (i != (this.ImportarSQL.ColumnasI.Count - 1))
                    RTBCodigoGenerado.Text += " . " + commilla + "','" + commilla;
            }
            RTBCodigoGenerado.Text += ". " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '200';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '500';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //por else
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += " $msg = '201';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //personazalida
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function "+NombreTablaSQL.Substring(0,4)+"pap" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$idusuario = is_null($f3->get('POST.idusuario')) ? 'T' : $f3->get('POST.idusuario');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$llave = is_null($f3->get('POST.llave')) ? 'T' : $f3->get('POST.llave');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_Usuariologueado->ValidaSession($idusuario, $llave,'pap" + NombreTabla + "',$f3)) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$Tipo = is_null($f3->get('POST.tipo')) ? 'T' : $f3->get('POST.tipo');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$procedure = is_null($f3->get('POST.procedure')) ? 'T' : $f3->get('POST.procedure');" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$" + this.ImportarSQL.ColumnasI[i] + " = is_null($f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "')) ? 'T' : $f3->get('POST.p" + this.ImportarSQL.ColumnasI[i] + "');" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$sql = " + commilla + "CALL " + commilla + " . $procedure . " + commilla + "('" + commilla + " . $Tipo . " + commilla + "','" + commilla;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += ". $" + this.ImportarSQL.ColumnasI[i];
                if (i != (this.ImportarSQL.ColumnasI.Count - 1))
                    RTBCodigoGenerado.Text += " . " + commilla + "','" + commilla;
            }
            RTBCodigoGenerado.Text += ". " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '200';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '500';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = $resulto;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} catch (PDOException $e) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode('{" + commilla + "error" + commilla + " : { " + commilla + "text" + commilla + ":' . $e->getMessage() . '}');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //por else
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += " $msg = '201';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //cerramos la clase
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion

        }
        public void CrearService()
        {
            string ComillaSimple, tipoDato, tipoSql;
            string commilla = @"""";
            string codBusqueda;
            string comillasIgual = @""" = '""".ToString() + @"""".Substring(1);
            string comillasSeguidas = @"""""";
            string like1 = @""" like '*""";
            string varT = comi + "T" + comi;
            //" + TXTSever.Text.ToString() + "
            string por = @"""*'""";
            ComillaSimple = @"""";
            tipoDato = "";
            tipoSql = "";
            //codigo para el modelo
            string NombreTabla = "";
            //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(5)));
            //else
            //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(6)));
            acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());
            RTBCodigoGenerado.Text += "import { Injectable } from '@angular/core';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { map, tap } from 'rxjs/operators';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { Observable } from 'rxjs';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { ProcedureParam } from '../models/usuariotoken.models';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { " + NombreTabla + "Model } from '../models/" + NombreTabla.ToLower() + ".models';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { environment } from '../../environments/environment';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "import { DatePipe } from '@angular/common';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const apiUlr = environment.apiUlr;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "@Injectable({  providedIn: 'root'})" + Environment.NewLine;
            RTBCodigoGenerado.Text += "export class S" + NombreTabla.ToLower() + "Service {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "constructor(private http: HttpClient,public datepipe: DatePipe) { }" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//" + NombreTabla.ToLower() + ": " + NombreTabla + "Model;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//select" + Environment.NewLine;
            RTBCodigoGenerado.Text += "sel" + NombreTabla + "(procedureParam: ProcedureParam) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const myheader = new HttpHeaders().set('Content-Type', 'application/form-data')" + Environment.NewLine;
            RTBCodigoGenerado.Text += "let body = new HttpParams();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('idusuario', localStorage.getItem('parmid')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('llave', localStorage.getItem('parmtoken')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//parametros" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo0', procedureParam.pCampo0);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor0', procedureParam.pValor0);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo1', procedureParam.pCampo1);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor1', procedureParam.pValor1);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo2', procedureParam.pCampo2);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor2', procedureParam.pValor2);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo3', procedureParam.pCampo3);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor3', procedureParam.pValor3);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo4', procedureParam.pCampo4);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor4', procedureParam.pValor4);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo5', procedureParam.pCampo5);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor5', procedureParam.pValor5);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo6', procedureParam.pCampo6);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor6', procedureParam.pValor6);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pCampo7', procedureParam.pCampo7);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('pValor7', procedureParam.pValor7);" + Environment.NewLine;

            RTBCodigoGenerado.Text += "//realizar consulta" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return this.http.post(apiUlr + 'sel" + NombreTabla.ToLower() + "', body)" + Environment.NewLine;
            RTBCodigoGenerado.Text += ".pipe(map((resp:any) => {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['info'] != null) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['mesaje'] != null) {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return resp['info'].item;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return resp;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('FAILD');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} else {console.log('error coneccion');return null;}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}));" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//termina sel" + NombreTabla + "" + Environment.NewLine;
            //create
            RTBCodigoGenerado.Text += "//create" + Environment.NewLine;
            RTBCodigoGenerado.Text += "add" + NombreTabla + "(" + NombreTabla.ToLower() + ": " + NombreTabla + "Model) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const myheader = new HttpHeaders().set('Content-Type', 'application/form-data')" + Environment.NewLine;
            RTBCodigoGenerado.Text += "let body = new HttpParams();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('idusuario', localStorage.getItem('parmid')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('llave', localStorage.getItem('parmtoken')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//parametros" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', this.datepipe.transform(" + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ", 'yyyy-MM-dd HH:mm:ss')||'');";
                else
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', " + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ".toString());";
            }
            //RTBCodigoGenerado.Text += "console.log('llamar el post');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//realizar consulta" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return this.http.post(apiUlr + 'add" + NombreTabla + "', body)" + Environment.NewLine;
            RTBCodigoGenerado.Text += ".pipe(map((resp:any) => {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "console.log(resp);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['info'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['mesaje'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return resp['info'].item;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return resp;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('FAILD');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('error coneccion');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}));" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//termina create" + Environment.NewLine;
            //read
            RTBCodigoGenerado.Text += "//read" + Environment.NewLine;
            RTBCodigoGenerado.Text += "get" + NombreTabla + "(" + NombreTabla.ToLower() + ": " + NombreTabla + "Model) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const myheader = new HttpHeaders().set('Content-Type', 'application/form-data')" + Environment.NewLine;
            RTBCodigoGenerado.Text += "let body = new HttpParams();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('idusuario', localStorage.getItem('parmid')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('llave', localStorage.getItem('parmtoken')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//parametros" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', this.datepipe.transform(" + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ", 'yyyy-MM-dd HH:mm:ss')||'');";
                else
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', " + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ".toString());";
            }
            RTBCodigoGenerado.Text += "console.log('llamar el post');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//realizar consulta" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return this.http.post(apiUlr + 'get" + NombreTabla + "', body)" + Environment.NewLine;
            RTBCodigoGenerado.Text += ".pipe(map((resp:any) => {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "console.log(resp);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['info'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['mesaje'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return resp['info'].item;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return resp;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('FAILD');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('error coneccion');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}));" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//termina read" + Environment.NewLine;
            //update
            RTBCodigoGenerado.Text += "//update" + Environment.NewLine;
            RTBCodigoGenerado.Text += "upd" + NombreTabla + "(" + NombreTabla.ToLower() + ": " + NombreTabla + "Model) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const myheader = new HttpHeaders().set('Content-Type', 'application/form-data')" + Environment.NewLine;
            RTBCodigoGenerado.Text += "let body = new HttpParams();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('idusuario', localStorage.getItem('parmid')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('llave', localStorage.getItem('parmtoken')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//parametros" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', this.datepipe.transform(" + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ", 'yyyy-MM-dd HH:mm:ss')||'');";
                else
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', " + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ".toString());";
            }
            RTBCodigoGenerado.Text += "console.log('llamar el post');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//realizar consulta" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return this.http.post(apiUlr + 'upd" + NombreTabla + "', body)" + Environment.NewLine;
            RTBCodigoGenerado.Text += ".pipe(map((resp:any) => {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "console.log(resp);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['info'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['mesaje'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return resp['info'].item;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return resp;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('FAILD');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('error coneccion');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}));" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//termina update" + Environment.NewLine;
            //personalizado
            RTBCodigoGenerado.Text += "//personalizado" + Environment.NewLine;
            RTBCodigoGenerado.Text += "pap" + NombreTabla + "(" + NombreTabla.ToLower() + ": " + NombreTabla + "Model,tipo:string,procedure:string) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "const myheader = new HttpHeaders().set('Content-Type', 'application/form-data')" + Environment.NewLine;
            RTBCodigoGenerado.Text += "let body = new HttpParams();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('idusuario', localStorage.getItem('parmid')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('llave', localStorage.getItem('parmtoken')||'');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('tipo', tipo);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "body = body.set('procedure', procedure);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//parametros" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString().Contains("fecha"))
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', this.datepipe.transform(" + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ", 'yyyy-MM-dd HH:mm:ss')||'');";
                else
                    RTBCodigoGenerado.Text += "body = body.set('p" + this.ImportarSQL.ColumnasI[i] + "', " + NombreTabla.ToLower() + "." + this.ImportarSQL.ColumnasI[i] + ".toString());";
            }
            RTBCodigoGenerado.Text += "console.log('llamar el post');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//realizar consulta" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return this.http.post(apiUlr + 'pap" + NombreTabla + "', body)" + Environment.NewLine;
            RTBCodigoGenerado.Text += ".pipe(map((resp:any) => {" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "console.log(resp);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['info'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (resp['mesaje'] != null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return resp['info'].item;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return resp;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('FAILD');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "console.log('error coneccion');" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}));" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//termina personalizado" + Environment.NewLine;
            //cerrar
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
        }



        private void guardarArchivoTexto_Click(object sender, EventArgs e)
        {
            string Nombreclase = "";
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "Entidades")
            {
                Nombreclase = "EDI" + LBTablas.SelectedValue;
            }
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "Controladoras")
            {
                Nombreclase = "C" + LBTablas.SelectedValue;
            }
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "WebMetodos")
            {
                Nombreclase = "WEBMETODO" + LBTablas.SelectedValue;
            }
            string nombre;
            try
            {
                nombre = TXTRutaArchivo.Text + @"\" + Nombreclase + ".cs";

                RTBCodigoGenerado.SaveFile(nombre, RichTextBoxStreamType.PlainText);
            }
            catch
            {
                MessageBox.Show("No se encontro la ruta para guardar este archivo");
            }
        }
        public string ParametroAcronimo = "_tbl";
        private void acronimoretorna(ref string NombreTabla,string NombreTablaOrigen)
        {
            //if (NombreTablaOrigen.Substring(0, 7) == ParametroAcronimo)
            //    NombreTabla = NombreTablaOrigen.Substring(7);
            //else
            //    NombreTabla = NombreTablaOrigen.Substring(7);
            NombreTabla = NombreTablaOrigen.Substring(7);
        }
        private void BTGenerarAutomaticamente_Click(object sender, EventArgs e)
        {
            BaseDatos._NombreBaseDatos = LBBaseDatos.SelectedValue.ToString();
            //BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            BaseDatos.ConnectionString = "Server=" + BaseDatos.NombreServidor + ";Database=" + BaseDatos.NombreBaseDatos + ";UID=" + BaseDatos.Cuenta + ";Password =" + BaseDatos.Password + ";";

            TXTWebservices.Text = BaseDatos.NombreBaseDatos;
            //Error2.Text = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            Error2.Text = "Server=" + BaseDatos.NombreServidor + ";Database=" + BaseDatos.NombreBaseDatos + ";UID=" + BaseDatos.Cuenta + ";Password =" + BaseDatos.Password + ";";
            #region cargar el listobox con los valores de la tabla
            LBTablas.Items.Clear();
            LBTablas.Refresh();
            try
            {
                DataSet ds = new DataSet("Tabla");
                ds = ImportarSQL.TablasMySqlServer(BaseDatos.ConnectionString, TXTWebservices.Text,ParametroAcronimo);
                dataGrid1.DataSource = ds.Tables["table"];
                dataGrid1.Refresh();
                LBTablas.DataSource = ds.Tables["table"];
                LBTablas.DisplayMember = "TABLE_NAME";
                LBTablas.ValueMember = "TABLE_NAME";
                LBTablas.Refresh();
                Error1.Text = "Conexion Exitosa... Puede continuar";
                Error1.ForeColor = Color.Blue;
                BaseDatos._Conectado = true;
                //--//

            }
            catch
            {
                Error1.Text = "La conexión ha fallado...";
                Error1.ForeColor = Color.Red;
                BaseDatos.Conectado = false;
            }
            #endregion
            //region gen
            #region crear las carpetas
            string directorio = TXTRutaArchivo.Text;
            BaseDatos.NombreBaseDatos = BaseDatos.NombreBaseDatos.ToString();
            if (Directory.Exists(directorio))
            {
                directorio = directorio + "\\cls" + BaseDatos.NombreBaseDatos.ToString();
                Directory.CreateDirectory(directorio + "\\controladores");
                Directory.CreateDirectory(directorio + "\\models");
                Directory.CreateDirectory(directorio + "\\services");
            }
            else
            {            //Runtimefolder is created         
                MessageBox.Show("Ingrese un directorio valido para guardar las clases");
            }
            BTNGenerarCodigo.Enabled = true;

            #endregion

            #region cargamos los campo
            BTNGenerarCodigo.Enabled = true;
            
            //creamos las entidades ok revizado
            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            {
                LBTablas.SelectedIndex = tab;
                BaseDatos.Tablas.Clear();
                BaseDatos.Tablas.Add(LBTablas.SelectedValue);
                string conect = Error2.Text;
                this.ImportarSQL.ColumnasI.Clear();
                this.ImportarSQL.TypoColumnasI.Clear();
                this.ImportarSQL.CargarColumnas(conect, BaseDatos.Tablas[0].ToString());
                this.LBCampos.Items.Clear();

                ImportarSQL.tablaNombre = LBTablas.SelectedValue.ToString();
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                RTBCodigoGenerado.Text = "";
                string NombreTabla = "";
                //if (LBTablas.SelectedValue.ToString().Substring(0, 3) == ParametroAcronimo)
                //    NombreTabla = LBTablas.SelectedValue.ToString().Substring(3);
                //else
                //    NombreTabla = LBTablas.SelectedValue.ToString();
                acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());
                CrearLasEntidades();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    RTBCodigoGenerado.SaveFile(directorio + "\\models\\" + NombreTabla + ".models.ts", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\models\\" + NombreTabla + ".models.ts", RichTextBoxStreamType.PlainText);
                }


            }
            //creamos el acceso a la data 
            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            {

                LBTablas.SelectedIndex = tab;
                BaseDatos.Tablas.Clear();
                BaseDatos.Tablas.Add(LBTablas.SelectedValue);
                string conect = Error2.Text;
                this.ImportarSQL.ColumnasI.Clear();
                this.ImportarSQL.TypoColumnasI.Clear();
                this.ImportarSQL.CargarColumnas(conect, BaseDatos.Tablas[0].ToString());
                this.LBCampos.Items.Clear();
                ImportarSQL.tablaNombre = LBTablas.SelectedValue.ToString();
                string NombreTabla = "";
                //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
                //    NombreTabla = LBTablas.SelectedValue.ToString().Substring(5);
                //else
                //    NombreTabla = LBTablas.SelectedValue.ToString().Substring(6);
                acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                RTBCodigoGenerado.Text = "";
                CrearLasControladoras();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    //RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + "_Ctrl.php", RichTextBoxStreamType.PlainText);
                    //RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + "_Ctrl.php", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + LBTablas.SelectedValue.ToString() + "_Ctrl.php", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + LBTablas.SelectedValue.ToString() + "_Ctrl.php", RichTextBoxStreamType.PlainText);
                }

            }
            //creamos el negocio estandas
            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            {

                LBTablas.SelectedIndex = tab;
                //BaseDatos.Tablas.Add(LBTablas.SelectedValue.ToString());                

                BaseDatos.Tablas.Clear();
                BaseDatos.Tablas.Add(LBTablas.SelectedValue);
                string conect = Error2.Text;
                this.ImportarSQL.ColumnasI.Clear();
                this.ImportarSQL.TypoColumnasI.Clear();
                this.ImportarSQL.CargarColumnas(conect, BaseDatos.Tablas[0].ToString());
                this.LBCampos.Items.Clear();


                ImportarSQL.tablaNombre = LBTablas.SelectedValue.ToString();
                string NombreTabla = "";
                //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
                //    NombreTabla = LBTablas.SelectedValue.ToString().Substring(5);
                //else
                //    NombreTabla = LBTablas.SelectedValue.ToString().Substring(6);
                acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());

                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                RTBCodigoGenerado.Text = "";
                CrearService();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    RTBCodigoGenerado.SaveFile(directorio + "\\services\\s" + NombreTabla + ".service.ts", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\services\\s" + NombreTabla + ".service.ts", RichTextBoxStreamType.PlainText);
                }



            }
            //creamos los procedimientos
            RTBCodigoGenerado.Text = "#procedures" + Environment.NewLine;
            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            {
                LBTablas.SelectedIndex = tab;
                BaseDatos.Tablas.Clear();
                BaseDatos.Tablas.Add(LBTablas.SelectedValue);
                this.ImportarSQL.ColumnasI.Clear();
                this.ImportarSQL.TypoColumnasI.Clear();
                this.ImportarSQL.CargarColumnasMysql(BaseDatos.ConnectionString, BaseDatos.Tablas[0].ToString());
                this.LBCampos.Items.Clear();
                ImportarSQL.tablaNombre = LBTablas.SelectedValue.ToString();
                string NombreTabla = "";
                //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "contrn")
                //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(5)));
                //else
                //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString()));
                NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString())).ToLower();
                string NombreTablaSQL = "";
                acronimoretorna(ref NombreTablaSQL, NombreTabla);
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                //dav
                RTBCodigoGenerado.Text += "DELIMITER //" + Environment.NewLine;
                RTBCodigoGenerado.Text += "CREATE PROCEDURE `"+ NombreTabla.Substring(0,4)+"pag" + NombreTablaSQL + "`(" + Environment.NewLine;
                RTBCodigoGenerado.Text += "IN `pTipo` VARCHAR(1)," + Environment.NewLine;
                //trabajando
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    RTBCodigoGenerado.Text += "IN `p" + this.ImportarSQL.ColumnasI[i] + "` " + this.ImportarSQL.TypoColum[i].ToString() + ",";
                //insert
                RTBCodigoGenerado.Text = RTBCodigoGenerado.Text.Substring(0, RTBCodigoGenerado.Text.Length - 1) + ")" + Environment.NewLine;
                RTBCodigoGenerado.Text += "begin" + Environment.NewLine;
                RTBCodigoGenerado.Text += "IF pTipo = 'C' then" + Environment.NewLine;
                RTBCodigoGenerado.Text += "IF p" + this.ImportarSQL.ColumnasI[0].ToString() + " = 0 then" + Environment.NewLine;
                RTBCodigoGenerado.Text += "SET p" + this.ImportarSQL.ColumnasI[0].ToString() + " = (SELECT(IFNULL(MAX(" + this.ImportarSQL.ColumnasI[0].ToString() + ") + 1, 1)) FROM " + NombreTabla + ");" + Environment.NewLine;
                RTBCodigoGenerado.Text += "END IF;" + Environment.NewLine;
                RTBCodigoGenerado.Text += "INSERT INTO " + NombreTabla + "(" + Environment.NewLine;
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i] + ",";
                //RTBCodigoGenerado.Text += "fecharegistro,filaestado)	values (" + Environment.NewLine;
                RTBCodigoGenerado.Text = RTBCodigoGenerado.Text.Substring(0, RTBCodigoGenerado.Text.Length-1)+")	values (" + Environment.NewLine;
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    RTBCodigoGenerado.Text += "p" + this.ImportarSQL.ColumnasI[i] + ",";
                //RTBCodigoGenerado.Text += "NOW(),1);" + Environment.NewLine;
                RTBCodigoGenerado.Text = RTBCodigoGenerado.Text.Substring(0, RTBCodigoGenerado.Text.Length - 1) + ");" + Environment.NewLine;
                RTBCodigoGenerado.Text += "SELECT p" + this.ImportarSQL.ColumnasI[0].ToString() + " AS " + this.ImportarSQL.ColumnasI[0].ToString() + ";" + Environment.NewLine;
                RTBCodigoGenerado.Text += "END IF;" + Environment.NewLine;
                //read
                RTBCodigoGenerado.Text += "IF pTipo = 'R' then" + Environment.NewLine;
                //RTBCodigoGenerado.Text += "SELECT * FROM " + NombreTabla + " WHERE p" + this.ImportarSQL.ColumnasI[0].ToString() + "=" + this.ImportarSQL.ColumnasI[0].ToString() + " AND filaestado=1;" + Environment.NewLine;
                RTBCodigoGenerado.Text += "SELECT * FROM " + NombreTabla + " WHERE p" + this.ImportarSQL.ColumnasI[0].ToString() + "=" + this.ImportarSQL.ColumnasI[0].ToString() + " and estadoregistro=1;" + Environment.NewLine;
                RTBCodigoGenerado.Text += "END IF;" + Environment.NewLine;
                //update
                RTBCodigoGenerado.Text += "IF pTipo = 'U' then" + Environment.NewLine;
                RTBCodigoGenerado.Text += "	update " + NombreTabla + " SET "+ Environment.NewLine ;                                
                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i]+"=p" + this.ImportarSQL.ColumnasI[i] + ",";
                RTBCodigoGenerado.Text = RTBCodigoGenerado.Text.Substring(0, RTBCodigoGenerado.Text.Length - 1) + Environment.NewLine;
                RTBCodigoGenerado.Text += "	WHERE p" + this.ImportarSQL.ColumnasI[0].ToString() + " = " + this.ImportarSQL.ColumnasI[0].ToString() + "; " + Environment.NewLine;
                RTBCodigoGenerado.Text += "SELECT p" + this.ImportarSQL.ColumnasI[0].ToString() + " AS " + this.ImportarSQL.ColumnasI[0].ToString() + ";" + Environment.NewLine;
                RTBCodigoGenerado.Text += "END IF;" + Environment.NewLine;
                //delete
                RTBCodigoGenerado.Text += "IF pTipo = 'D' then" + Environment.NewLine;
                RTBCodigoGenerado.Text += "	delete from " + NombreTabla + " WHERE p" + this.ImportarSQL.ColumnasI[0].ToString() + " = " + this.ImportarSQL.ColumnasI[0].ToString() + "; " + Environment.NewLine;
                RTBCodigoGenerado.Text += "END IF;" + Environment.NewLine;
                RTBCodigoGenerado.Text += "END//" + Environment.NewLine;
                RTBCodigoGenerado.Text += "DELIMITER ;" + Environment.NewLine;
                RTBCodigoGenerado.Text += "" + Environment.NewLine;
            }
            RTBCodigoGenerado.SaveFile(directorio + "\\procedimientos.sql", RichTextBoxStreamType.PlainText);
            RTBCodigoGenerado.SaveFile(directorio + "\\procedimientos.sql", RichTextBoxStreamType.PlainText);
            //creamos el ruteo
            RTBCodigoGenerado.Text = "[routes]" + Environment.NewLine;
            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            {
                LBTablas.SelectedIndex = tab;
                string NombreTabla = "";
                //if (LBTablas.SelectedValue.ToString().Substring(0, 5) == "almcl")
                //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(5)));
                //else
                //    NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(6)));
                acronimoretorna(ref NombreTabla, LBTablas.SelectedValue.ToString());
                RTBCodigoGenerado.Text += "#" + NombreTabla + "s" + Environment.NewLine;
                RTBCodigoGenerado.Text += "POST /sel" + NombreTabla + " = " + LBTablas.SelectedValue.ToString() + "_Ctrl->sel" + NombreTabla + Environment.NewLine;
                RTBCodigoGenerado.Text += "POST /add" + NombreTabla + " = " +  LBTablas.SelectedValue.ToString() + "_Ctrl->add" + NombreTabla + Environment.NewLine;
                RTBCodigoGenerado.Text += "POST /get" + NombreTabla + " = " +  LBTablas.SelectedValue.ToString() + "_Ctrl->get" + NombreTabla + Environment.NewLine;
                RTBCodigoGenerado.Text += "POST /upd" + NombreTabla + " = " +  LBTablas.SelectedValue.ToString() + "_Ctrl->upd" + NombreTabla + Environment.NewLine;
                RTBCodigoGenerado.Text += "POST /pap" + NombreTabla + " = " +  LBTablas.SelectedValue.ToString() + "_Ctrl->pap" + NombreTabla + Environment.NewLine;
            }
            RTBCodigoGenerado.SaveFile(directorio + "\\routes.ini", RichTextBoxStreamType.PlainText);
            RTBCodigoGenerado.SaveFile(directorio + "\\routes.ini", RichTextBoxStreamType.PlainText);

            #endregion
            NOTA();
            MessageBox.Show("Creacion realizada correctamente");
            this.Dispose();
        }
        protected enum Tone
        {
            REST = 0,
            GbelowC = 196,
            A = 220,
            Asharp = 233,
            B = 247,
            C = 262,
            Csharp = 277,
            D = 294,
            Dsharp = 311,
            E = 330,
            F = 349,
            Fsharp = 370,
            G = 392,
            Gsharp = 415,
        }
        // Define the duration of a note in units of milliseconds.
        protected enum Duration
        {
            WHOLE = 1600,
            HALF = WHOLE / 2,
            QUARTER = HALF / 2,
            EIGHTH = QUARTER / 2,
            SIXTEENTH = EIGHTH / 2,
        }

        // Define a note as a frequency (tone) and the amount of 
        // time (duration) the note plays.
        protected struct Note
        {
            Tone toneVal;
            Duration durVal;

            // Define a constructor to create a specific note.
            public Note(Tone frequency, Duration time)
            {
                toneVal = frequency;
                durVal = time;
            }

            // Define properties to return the note's tone and duration.
            public Tone NoteTone { get { return toneVal; } }
            public Duration NoteDuration { get { return durVal; } }
        }
        void NOTA()
        {
            Note[] Mary =
        {
        new Note(Tone.B, Duration.QUARTER),
        new Note(Tone.A, Duration.QUARTER),
        new Note(Tone.GbelowC, Duration.QUARTER),
        new Note(Tone.A, Duration.QUARTER),
        new Note(Tone.B, Duration.QUARTER),
        new Note(Tone.B, Duration.QUARTER),
        new Note(Tone.B, Duration.HALF),
        new Note(Tone.A, Duration.QUARTER),
        new Note(Tone.A, Duration.QUARTER),
        new Note(Tone.A, Duration.HALF),
        new Note(Tone.B, Duration.QUARTER),
        new Note(Tone.D, Duration.QUARTER),
        new Note(Tone.D, Duration.HALF)
        };
            // Play the song
            Play(Mary);
        }
        protected static void Play(Note[] tune)
        {
            foreach (Note n in tune)
            {
                if (n.NoteTone == Tone.REST)
                    Thread.Sleep((int)n.NoteDuration);
                else
                    Console.Beep((int)n.NoteTone, (int)n.NoteDuration);
            }
        }

        private string CrearDTO(string nombre, string ruta)
        {
            //Checks that already folder of this name exists or not

            DataSet ds = new DataSet();
            ds.Tables.Add(nombre);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " + nombre, Error2.Text.ToString());
            //sda.Fill(ds,nombre);
            sda.FillSchema(ds, SchemaType.Mapped, nombre);
            ds.WriteXmlSchema(ruta + "DTO" + nombre + ".xsd");

            return nombre;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            TXTRutaArchivo.Text = folderBrowserDialog1.SelectedPath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}