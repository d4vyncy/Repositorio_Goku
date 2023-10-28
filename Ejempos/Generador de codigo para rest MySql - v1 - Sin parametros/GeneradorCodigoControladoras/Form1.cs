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

namespace GeneradorCodigoControladoras
{
    public partial class Form1 : Form
    {
        public Entidades.EBaseDatos BaseDatos = new GeneradorCodigoControladoras.Entidades.EBaseDatos();
        public Entidades.EImportarSQL ImportarSQL = new GeneradorCodigoControladoras.Entidades.EImportarSQL();
        public Entidades.EOrdenar eOrdenar = new GeneradorCodigoControladoras.Entidades.EOrdenar();
        public DataSet dts = new DataSet();
        public string CadenaConexion;
        public Form1()
        {
            InitializeComponent();
            if (DateTime.Now > Convert.ToDateTime("01/01/2021"))
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
            BaseDatos._NombreBaseDatos = "master";
            BaseDatos._NombreServidor = TXTSever.Text;
            BaseDatos._Password = TXTPass.Text;
            Error2.Text = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            try
            {
                DataSet dsResultados = new DataSet();
                SqlConnection MiConexion = new SqlConnection(BaseDatos.ConnectionString);
                SqlDataAdapter MiDataAdapter = new SqlDataAdapter();
                MiConexion.Open();
                string Sql = "sp_databases";
                SqlCommand MiCommand = new SqlCommand(Sql, MiConexion);
                MiCommand.CommandType = CommandType.Text;
                MiDataAdapter.SelectCommand = MiCommand;
                DataSet ds = new DataSet("Tabla");
                MiDataAdapter.Fill(ds);
                MiConexion.Close();

                LBBaseDatos.DataSource = ds.Tables["table"];
                LBBaseDatos.DisplayMember = "DATABASE_NAME";
                LBBaseDatos.ValueMember = "DATABASE_NAME";
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
                ds = ImportarSQL.TablasSqlServer(BaseDatos.ConnectionString);
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
                ds = ImportarSQL.TablasSqlServer(Error2.Text.ToString());
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
            string tipoDato, valorDato;
            #region //Creamos la cabecera de la entidad
            RTBCodigoGenerado.Text += "using System;" + Environment.NewLine + "using System.Collections.Generic;" + Environment.NewLine + "using System.Linq;" + Environment.NewLine + "using System.Text;" + Environment.NewLine;
            //+Environment.NewLine + Environment.NewLine + "/// <summary>" + Environment.NewLine + "/// Codigo generado por D4VYNCY@HOTMAIL.COM &" + Environment.NewLine + "/// </summary>;" + Environment.NewLine;
            RTBCodigoGenerado.Text += Environment.NewLine;
            RTBCodigoGenerado.Text += "namespace twa" + BaseDatos.NombreBaseDatos.ToString() + ".ControlDB.Entity " + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += Environment.NewLine;
            RTBCodigoGenerado.Text += "public class cls" + ImportarSQL.tablaNombre + "BE" + Environment.NewLine + "{" + Environment.NewLine;

            //LLenamos los campos atributos de la entidad
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                //tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                tipoDato = validarDato(this.ImportarSQL.TypoColumnasI[i].ToString());
                RTBCodigoGenerado.Text += "public " + tipoDato + " " + this.ImportarSQL.ColumnasI[i] + " { get; set; }" + Environment.NewLine;
            }

            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}";
            #endregion
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
            if (LBTablas.SelectedValue.ToString().Substring(0, 2) == "cl")
                NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(2)));
            else
                NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(3)));

            RTBCodigoGenerado.Text += "//gen " + DateTime.Now + " dst" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<?php" + Environment.NewLine;
            RTBCodigoGenerado.Text += "class " + NombreTabla + "_Ctrl" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public $M_" + NombreTabla + " = null;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function __construct()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$this->M_" + NombreTabla + " = new M_" + NombreTabla + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            #region para llenar los datos
            //selecionamos registros
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function sel" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
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
            RTBCodigoGenerado.Text += "$sql = "+ commilla + "CALL pviciudad('" + commilla + " . $pCampo0 . " + commilla + "','" + commilla + ". $pValor0 . " + commilla + "','" + commilla + ". $pCampo1 . " + commilla + "','" + commilla + ". $pValor1 . " + commilla + "','" + commilla + ". $pCampo2 . " + commilla + "','" + commilla + ". $pValor2 . " + commilla + "','" + commilla + ". $pCampo3 . " + commilla + "','" + commilla + ". $pValor3 . " + commilla + "','" + commilla + ". $pCampo4 . " + commilla + "','" + commilla + ". $pValor4 . " + commilla + "','" + commilla + ". $pCampo5 . " + commilla + "','" + commilla + ". $pValor5 . " + commilla + "','" + commilla + ". $pCampo6 . " + commilla + "','" + commilla + ". $pValor6 . " + commilla + "','" + commilla + ". $pCampo7 . " + commilla + "','" + commilla + ". $pValor7 . " + commilla + "'); " + commilla + "; " + Environment.NewLine;
            RTBCodigoGenerado.Text += "try {" + Environment.NewLine;            
            RTBCodigoGenerado.Text += "$resulto = $f3->get('DB')->exec($sql);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = 'Lista de " + NombreTabla + "s';" + Environment.NewLine;
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
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //creamos los metodos CRUD
            //create
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function add" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$this->M_" + NombreTabla + "->set('" + this.ImportarSQL.ColumnasI[i] + "', $f3->get('POST." + this.ImportarSQL.ColumnasI[i] + "'));" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$this->M_" + NombreTabla + "->save();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => '" + ImportarSQL.tablaNombre + " creado'," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            for (int i = 0; i < 1; i++)
            {
                RTBCodigoGenerado.Text += "'" + this.ImportarSQL.ColumnasI[i] + "' => $this->M_" + NombreTabla + "->get('" + this.ImportarSQL.ColumnasI[i] + "')" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //read
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function get" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            for (int i = 0; i < 1; i++)
            {
                RTBCodigoGenerado.Text += "$" + this.ImportarSQL.ColumnasI[i] + "= $f3->get('PARAMS." + this.ImportarSQL.ColumnasI[i] + "');" + Environment.NewLine;
                RTBCodigoGenerado.Text += "$this->M_" + NombreTabla + "->load(['" + this.ImportarSQL.ColumnasI[i] + "=?', $" + this.ImportarSQL.ColumnasI[i] + "]);" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "$items = array();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if ($this->M_"+ NombreTabla + "->loaded() > 0) {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '" + NombreTabla + " encontrado';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$items=$this->M_" + NombreTabla + "->cast();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "} else {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "$msg = '" + NombreTabla + " no existe';" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => $msg," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'item' => $items" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //update
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function upd" + NombreTabla + "($f3)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += "$this->M_" + NombreTabla + "->set('" + this.ImportarSQL.ColumnasI[i] + "', $f3->get('POST." + this.ImportarSQL.ColumnasI[i] + "'));" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "echo json_encode([" + Environment.NewLine;
            RTBCodigoGenerado.Text += "'mesaje' => '" + ImportarSQL.tablaNombre + " creado'," + Environment.NewLine;
            RTBCodigoGenerado.Text += "'info' => [" + Environment.NewLine;
            for (int i = 0; i < 1; i++)
            {
                RTBCodigoGenerado.Text += "'" + this.ImportarSQL.ColumnasI[i] + "' => $this->M_" + NombreTabla + "->get('" + this.ImportarSQL.ColumnasI[i] + "')" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "]);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //cerramos la clase
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion

        }
        public void CrearElNegocio()
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
            if (LBTablas.SelectedValue.ToString().Substring(0, 2) == "cl")
                NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(2)));
            else
                NombreTabla = (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(LBTablas.SelectedValue.ToString().Substring(3)));
            RTBCodigoGenerado.Text += "<?php" + Environment.NewLine;
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "class M_" + NombreTabla + " extends \\DB\\SQL\\Mapper{" + Environment.NewLine ;
            RTBCodigoGenerado.Text += "" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public function __construct()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parent::__construct( \\Base::instance()->get('DB'), '"+ LBTablas.SelectedValue.ToString() + "' );" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
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

        private void BTGenerarAutomaticamente_Click(object sender, EventArgs e)
        {



            //region boton conectar
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
                ds = ImportarSQL.TablasSqlServer(BaseDatos.ConnectionString);
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
                Directory.CreateDirectory(directorio + "\\Entity");
                Directory.CreateDirectory(directorio + "\\modelos");
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
                CrearLasEntidades();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    RTBCodigoGenerado.SaveFile(directorio + "\\Entity\\cls" + ImportarSQL.tablaNombre.ToString() + "BE.cs", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\Entity\\cls" + ImportarSQL.tablaNombre.ToString() + "BE.cs", RichTextBoxStreamType.PlainText);
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
                if (LBTablas.SelectedValue.ToString().Substring(0, 2) == "cl")
                    NombreTabla = LBTablas.SelectedValue.ToString().Substring(2);
                else
                    NombreTabla = LBTablas.SelectedValue.ToString().Substring(3);

                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                RTBCodigoGenerado.Text = "";
                CrearLasControladoras();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + "_Ctrl.php", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\controladores\\" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + "_Ctrl.php", RichTextBoxStreamType.PlainText);
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
                if (LBTablas.SelectedValue.ToString().Substring(0, 2) == "cl")
                    NombreTabla = LBTablas.SelectedValue.ToString().Substring(2);
                else
                    NombreTabla = LBTablas.SelectedValue.ToString().Substring(3);

                for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                }
                RTBCodigoGenerado.Text = "";
                CrearElNegocio();
                if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                {
                    RTBCodigoGenerado.SaveFile(directorio + "\\modelos\\M_" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + ".php", RichTextBoxStreamType.PlainText);
                    RTBCodigoGenerado.SaveFile(directorio + "\\modelos\\M_" + (CultureInfo.InvariantCulture.TextInfo.ToTitleCase(NombreTabla)) + ".php", RichTextBoxStreamType.PlainText);
                }



            }

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