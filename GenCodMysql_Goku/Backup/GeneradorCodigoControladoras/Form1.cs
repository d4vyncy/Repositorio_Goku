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
            TXTWebservices.Text = BaseDatos.NombreBaseDatos.Substring(2);
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
            if( CLBSeleccionCodigo.SelectedItem.ToString()=="Entidades")
            {
            //creamos las entidades de los web services
            CrearLasEntidades();
            }
            if( CLBSeleccionCodigo.SelectedItem.ToString()=="Controladoras")
            {
            //creamos las controladoras del web services
            CrearLasControladoras();
            }
            if (CLBSeleccionCodigo.SelectedItem.ToString() == "WebMetodos")
            {
                //Creamos los web metodos
                CrearLosWebMetodos();
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
            int num,num1;
            num = Convert.ToInt32( DateTime.Today.Day.ToString());
            num = num * Convert.ToInt32( DateTime.Today.Month.ToString());
            num1 = Convert.ToInt32( DateTime.Today.Year.ToString());
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
            BTNGenerarCodigo.Enabled = true;
            BaseDatos.Tablas.Clear();
            BaseDatos.Tablas.Add(LBTablas.SelectedValue);
            string conect = Error2.Text;
            this.ImportarSQL.ColumnasI.Clear();
            this.ImportarSQL.TypoColumnasI.Clear();            
            this.ImportarSQL.CargarColumnas(conect, BaseDatos.Tablas[0].ToString());
            this.LBCampos.Items.Clear();            
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                    this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);
                    
                }
            }
        }

        void GenerarWebServices()
        {
            #region cabecera del web metodo
            RTBCodigoGenerado.Text = "CODIGO GENERADO POR D4VYNCY@HOTMAIL.COM";
            RTBCodigoGenerado.Text += "using System;";
            RTBCodigoGenerado.Text += "using System.Web;";
            RTBCodigoGenerado.Text += "using System.Web.Services;";
            RTBCodigoGenerado.Text += "using System.Web.Services.Protocols;";
            RTBCodigoGenerado.Text += "[WebService(Namespace =" + @"" +  "http://tempuri.org/" + @"" +")] ";
            RTBCodigoGenerado.Text += "[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]";
            RTBCodigoGenerado.Text += "public class Service : System.Web.Services.WebService";
            RTBCodigoGenerado.Text += "{";
            RTBCodigoGenerado.Text += "public Service () {";
            RTBCodigoGenerado.Text += "//GENERADOR DE WEB SERVICES V 1.0";
            RTBCodigoGenerado.Text += "}";
            #endregion
        #region generamos los web metodos para cada una de las tablas
            //RTBCodigoGenerado.Text += "#region " + LBTablas.Items.GetEnumerator;
        #endregion
 //#region asignatura
 //   [WebMethod(Description = "Llenar DTOasignatura")]
 //   //LLENAR EL DTOasignatura
    
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
        public string TipoDeDatoDataRow(string dato,string NombreCampo,string NombreTabla)
        {
            if (dato == "text")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "].ToString();"; }
            if (dato == "varchar")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "].ToString();"; }
            if (dato == "numeric")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDecimal(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Int32")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToInt32(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Int16")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToInt16(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Decimal")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDecimal(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Char")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "String")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Text")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToString(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "DateTime")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDateTime(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Boolean")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToBoolean(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            
            if (dato == "Double")
            { dato = "edi" + NombreTabla + "._" + NombreCampo + " = Convert.ToDouble(dtr" + NombreTabla + "[" + comi + NombreCampo + comi + "]);"; }
            

            return dato;
        }
        #endregion
        public void CrearLasEntidades()
        {
            string tipoDato, valorDato;
            #region //Creamos la cabecera de la entidad
            RTBCodigoGenerado.Text += Environment.NewLine + "using System;" + Environment.NewLine + "using System.Data;" + Environment.NewLine + "using System.Configuration;" + Environment.NewLine + "using System.Web;" + Environment.NewLine + "using System.Web.Security;" + Environment.NewLine + "using System.Web.UI;" + Environment.NewLine + "using System.Web.UI.WebControls;" + Environment.NewLine + "using System.Web.UI.WebControls.WebParts;" + Environment.NewLine + "using System.Web.UI.HtmlControls;" + Environment.NewLine + Environment.NewLine + "/// <summary>" + Environment.NewLine + "/// Codigo generado por D4VYNCY@HOTMAIL.COM &" + Environment.NewLine + "/// </summary>;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public class EDI" + LBTablas.SelectedValue + "" + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "#region Atributos de la Entidad" + LBTablas.SelectedValue + Environment.NewLine;
            //LLenamos los campos atributos de la entidad
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                //tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                tipoDato = validarDato(this.ImportarSQL.TypoColumnasI[i].ToString());
                RTBCodigoGenerado.Text += "private " + tipoDato + " " + this.ImportarSQL.ColumnasI[i] + ";" + Environment.NewLine;
            }
             RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
             RTBCodigoGenerado.Text += "public EDI" + LBTablas.SelectedValue + "()" + Environment.NewLine + "{" + Environment.NewLine;  
            //LLenamos los campos de la entidad con sus valores iniciales
             RTBCodigoGenerado.Text += "#region inicializacion de datos" + Environment.NewLine;  
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                valorDato = valorInicialDato(tipoDato);
                RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i] + " = " + valorDato + ";" +Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "#region publics para recuperar los datos (get/set)" + Environment.NewLine;  
            //cargamos los metodos de get y set para las entidades
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                valorDato = valorInicialDato(tipoDato);
                tipoDato = validarDato(tipoDato);
                RTBCodigoGenerado.Text += "public " + tipoDato + " _" + this.ImportarSQL.ColumnasI[i] + " { get { return  " + this.ImportarSQL.ColumnasI[i] + ";}set {" + this.ImportarSQL.ColumnasI[i] + " = value; } }" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}";            
            #endregion
        }
        string comi = @"""";
        public void CrearLasControladoras()
        {
            string ComillaSimple,tipoDato,tipoSql;
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
            RTBCodigoGenerado.Text += Environment.NewLine + "using System;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Data;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Configuration;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.Security;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.UI;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.UI.WebControls;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.UI.WebControls.WebParts;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.UI.HtmlControls;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using Microsoft.ApplicationBlocks.Data;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Data.SqlClient;" + Environment.NewLine;
            RTBCodigoGenerado.Text += Environment.NewLine + "/// <summary>" + Environment.NewLine + "/// Codigo generado por D4VYNCY@HOTMAIL.COM &" + Environment.NewLine + "/// </summary>;" + Environment.NewLine;

            RTBCodigoGenerado.Text += "public class C" + LBTablas.SelectedValue + "" + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "#region creacion y declaracion de variables de inicio" + Environment.NewLine;
                RTBCodigoGenerado.Text += "//DTO" + Environment.NewLine;
                RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " dto" + LBTablas.SelectedValue + ";" + Environment.NewLine;
                RTBCodigoGenerado.Text += "//clases" + Environment.NewLine;
                RTBCodigoGenerado.Text += "public EDI" + LBTablas.SelectedValue + " edi" + LBTablas.SelectedValue + ";" + Environment.NewLine;
                RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;

            RTBCodigoGenerado.Text += "public C" + LBTablas.SelectedValue + "()" + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "#region datos principales de inicio" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "edi" + LBTablas.SelectedValue + " = new EDI" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//cargamos los datos principales" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = Llenar_DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            
            #region para llenar los datos del dto
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Llenar_DTO" + LBTablas.SelectedValue + "()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string cadena = ConfigurationManager.ConnectionStrings[1].ConnectionString;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlHelper.FillDataset(cadena, CommandType.StoredProcedure, " + @"""" + "PAS_" + LBTablas.SelectedValue + "" + @"""" + ", dto" + LBTablas.SelectedValue + ", new string[] { " + @"""" + "" + LBTablas.SelectedValue + "" + @"""" + " });" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return dto" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            #endregion
            #region llenar los datos hacer modificados
            RTBCodigoGenerado.Text += " //Llenamos los datos hacer modificados del " + LBTablas.SelectedValue + Environment.NewLine;
            RTBCodigoGenerado.Text += "public int LlenarDatos_" + LBTablas.SelectedValue + "(int " + this.ImportarSQL.ColumnasI[0] + ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string cadena = ConfigurationManager.ConnectionStrings[1].ConnectionString;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlDataReader dtr" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dtr" + LBTablas.SelectedValue + " = SqlHelper.ExecuteReader(cadena, CommandType.Text, " + comi + " select top 1 * from VIS_" + LBTablas.SelectedValue + " where Codigo" + LBTablas.SelectedValue + "=" + comi + " + Codigo" + LBTablas.SelectedValue + ".ToString());" + Environment.NewLine;
            RTBCodigoGenerado.Text += "try" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (dtr" + LBTablas.SelectedValue + ".Read())" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;

            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                RTBCodigoGenerado.Text += TipoDeDatoDataRow(this.ImportarSQL.TypoColumnasI[i].ToString(), this.ImportarSQL.ColumnasI[i].ToString(), LBTablas.SelectedValue.ToString()) + Environment.NewLine;
                //RTBCodigoGenerado.Text += "edi" + LBTablas.SelectedValue + "._" + this.ImportarSQL.ColumnasI[i] + " = dtr" + LBTablas.SelectedValue + "[" + comi + this.ImportarSQL.ColumnasI[i] + comi + "];" + Environment.NewLine;
            }
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "catch" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dtr" + LBTablas.SelectedValue + ".Close();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return Codigo" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;





            //RTBCodigoGenerado.Text += "#region llenar datos" + Environment.NewLine;
            //LLenamos los campos de la tabla hacer llenados            
            //for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            //{
            //    RTBCodigoGenerado.Text += "edi" + LBTablas.SelectedValue + "._" + this.ImportarSQL.ColumnasI[i] + " = row" + LBTablas.SelectedValue + "[0]." + this.ImportarSQL.ColumnasI[i] + ";" + Environment.NewLine;
            //}
            //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return " + this.ImportarSQL.ColumnasI[0] + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;

             #endregion
            //#region llenar los datos hacer modificados
            //RTBCodigoGenerado.Text += " //Llenamos los datos hacer modificados del " + LBTablas.SelectedValue + Environment.NewLine;
            //RTBCodigoGenerado.Text += "public int LlenarDatos_" + LBTablas.SelectedValue + "(int " + this.ImportarSQL.ColumnasI[0] + ")" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "string condicion = dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + this.ImportarSQL.ColumnasI[0] + "Column.Caption + " + comillasIgual + " + " + this.ImportarSQL.ColumnasI[0] + ".ToString() + " + comillasSeguidas + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + " Dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] row" + LBTablas.SelectedValue + " = (DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[])dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Select(condicion);" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "Dto" + LBTablas.SelectedValue + ".Merge(row" + LBTablas.SelectedValue + ");" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "#region llenar datos" + Environment.NewLine;
            ////LLenamos los campos de la tabla hacer llenados            
            //for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            //{
            //    RTBCodigoGenerado.Text += "edi" + LBTablas.SelectedValue + "._" + this.ImportarSQL.ColumnasI[i] + " = row" + LBTablas.SelectedValue + "[0]." + this.ImportarSQL.ColumnasI[i] + ";" + Environment.NewLine;
            //}
            //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return " + this.ImportarSQL.ColumnasI[0] + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            //#endregion
            #region para adicionar datos a la tabla
            RTBCodigoGenerado.Text += "public int Adicionar_" + LBTablas.SelectedValue + "(";
            //for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            //{
            //    if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
            //    {
            //        if (i > 1)
            //        { RTBCodigoGenerado.Text += ","; }
            //        tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
            //        tipoDato = validarDato(tipoDato);
            //        RTBCodigoGenerado.Text += tipoDato + " " + this.ImportarSQL.ColumnasI[i];
            //    }
            //}
            RTBCodigoGenerado.Text += ")" + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string cadena = ConfigurationManager.ConnectionStrings[1].ConnectionString;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" +LBTablas.SelectedValue + " = new DTO" +LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlParameter[] parametros;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = SqlHelperParameterCache.GetCachedParameterSet(cadena, " + ComillaSimple + "Adicionar_" + LBTablas.SelectedValue+ComillaSimple+");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (parametros == null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = new SqlParameter[] {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "new SqlParameter(" + ComillaSimple + "@" + ComillaSimple + "+dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Codigo" + LBTablas.SelectedValue + "Column.ToString(),SqlDbType.Int,0,ParameterDirection.Output,false, 0, 0, " + ComillaSimple + "@" + ComillaSimple + "+dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Codigo" + LBTablas.SelectedValue + "Column.ToString(), DataRowVersion.Current, null)" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 0)
                    { RTBCodigoGenerado.Text += ","; }
                    tipoSql = datoSQL(this.ImportarSQL.TypoColumnasI[i].ToString());                    
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += "new SqlParameter(" + ComillaSimple + "@" + ComillaSimple + " + dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + this.ImportarSQL.ColumnasI[i] + "Column.ToString(), SqlDbType." + tipoSql + ")";
                }
            }
            RTBCodigoGenerado.Text += "};" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlHelperParameterCache.CacheParameterSet(cadena, " + @"""" + "Adicionar_" +LBTablas.SelectedValue + @"""" + ", parametros);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "};" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[0].Value = 0;" + Environment.NewLine;
            //For creado para los parametros
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {                    
                    tipoSql = datoSQL(this.ImportarSQL.TypoColumnasI[i].ToString());                    
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += "parametros[" + i + "].Value = edi" + LBTablas.SelectedValue + "._" + this.ImportarSQL.ColumnasI[i] + ";" + Environment.NewLine;
                }
            }
            RTBCodigoGenerado.Text += "SqlHelper.ExecuteNonQuery(cadena, CommandType.StoredProcedure, " + @"""" +"PAI_" + LBTablas.SelectedValue + @"""" + ", parametros);"+ Environment.NewLine;
            RTBCodigoGenerado.Text += "int CODIGO = (Int32)parametros[0].Value;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return CODIGO;" + Environment.NewLine + "}"+ Environment.NewLine;
            #endregion
            #region para Modificar los datos de la tabla
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Modificar_" + LBTablas.SelectedValue + "(" ;
            //for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            //{
            //    if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
            //    {
            //        if (i > 0)
            //        { RTBCodigoGenerado.Text += ","; }
            //        tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
            //        tipoDato = validarDato(tipoDato);
            //        RTBCodigoGenerado.Text += tipoDato + " " + this.ImportarSQL.ColumnasI[i];
            //    }
            //}
            RTBCodigoGenerado.Text += ")" + Environment.NewLine + "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string cadena = ConfigurationManager.ConnectionStrings[1].ConnectionString;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" +LBTablas.SelectedValue + " = new DTO" +LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlParameter[] parametros;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = SqlHelperParameterCache.GetCachedParameterSet(cadena, " + ComillaSimple + "Modificar_" + LBTablas.SelectedValue+ComillaSimple+");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (parametros == null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = new SqlParameter[] {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "new SqlParameter()";
            //cremos los parametros iniciales
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 0)
                    { RTBCodigoGenerado.Text += ","; }                    
                    RTBCodigoGenerado.Text += "new SqlParameter()" ;
                }
            }
            RTBCodigoGenerado.Text += "};" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlHelperParameterCache.CacheParameterSet(cadena, " + @"""" + "Modificar_" + LBTablas.SelectedValue + @"""" + ", parametros);" + Environment.NewLine;

            //nombramos los parametros
            RTBCodigoGenerado.Text += "parametros[0].ParameterName = " + ComillaSimple + "@" + ComillaSimple + "+ dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + "Codigo" + LBTablas.SelectedValue + "Column.ToString();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[0].SqlDbType = SqlDbType.Int;" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {                    
                    tipoSql = datoSQL(this.ImportarSQL.TypoColumnasI[i].ToString());                    
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += "parametros["+ i.ToString() +"].ParameterName = " + ComillaSimple + "@" + ComillaSimple + "+ dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + this.ImportarSQL.ColumnasI[i] + "Column.ToString();" + Environment.NewLine;
                }
            }
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            RTBCodigoGenerado.Text += "parametros[0].Value = edi" + LBTablas.SelectedValue + "._Codigo"+LBTablas.SelectedValue + ";" + Environment.NewLine;
            //For creado para los parametros
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {                    
                    tipoSql = datoSQL(this.ImportarSQL.TypoColumnasI[i].ToString());                    
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += "parametros[" + i + "].Value = edi" + LBTablas.SelectedValue + "._" + this.ImportarSQL.ColumnasI[i] + ";" + Environment.NewLine;
                }
            }
            RTBCodigoGenerado.Text += "SqlHelper.ExecuteNonQuery(cadena, CommandType.StoredProcedure, " + @"""" +"PAU_" + LBTablas.SelectedValue + @"""" + ", parametros);"+ Environment.NewLine;            
            RTBCodigoGenerado.Text += "return Llenar_DTO" + LBTablas.SelectedValue +"();" + Environment.NewLine + "}" + Environment.NewLine ;
            #endregion
            #region//Creamos la zona de buscar datos en la tabla por un parametro
            //buscar 
            //RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Buscar_" + LBTablas.SelectedValue + "(string dato)" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "string condicion = dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + codBusqueda + "Column.Caption + " + like1 + " + dato + " + por + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "string orden = dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + codBusqueda + "Column.Caption;" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + " Dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] row" + LBTablas.SelectedValue + " = (DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[])dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Select(condicion, orden);" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "Dto" + LBTablas.SelectedValue + ".Merge(row" + LBTablas.SelectedValue + ");" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return Dto" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;

            //Ultima Zona modifica
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Buscar_" + LBTablas.SelectedValue + "(string dato,string orden,string orden1)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string condicion = " + commilla + commilla + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (orden != " + commilla + commilla + ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (orden.Substring(0, 1) == " + commilla + "C" + commilla + ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "condicion = orden.Substring(1) + " + commilla + " like '*" + commilla + " + dato + " + commilla + "*'" + commilla + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (orden.Substring(0, 1) == " + commilla + "N" + commilla + ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (dato != " + commilla + "" + commilla + ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "condicion = orden.Substring(1) + " + commilla + " = " + commilla + " + dato " + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "condicion = orden.Substring(1) + " + commilla + " > 0" + commilla + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "else" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "condicion = orden.Substring(1) + " + commilla + " = '#" + commilla + " + dato + " + commilla + "#'" + commilla + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            RTBCodigoGenerado.Text += "//DESC + ASC" + Environment.NewLine;
            RTBCodigoGenerado.Text += "orden = orden.Substring(1) + " + commilla + " " + commilla + " + orden1;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + " Dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] row" + LBTablas.SelectedValue + " = (DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[])dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Select(condicion, orden);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "Dto" + LBTablas.SelectedValue + ".Merge(row" + LBTablas.SelectedValue + ");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return Dto" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;

            #region datos adicionales
            //                " + commilla + "            
            //Llenar DDL
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] LlenarDDL" + LBTablas.SelectedValue + "()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "string " + this.ImportarSQL.ColumnasI[1].ToString() + " = dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "." + this.ImportarSQL.ColumnasI[1].ToString() + "Column.ToString();" + Environment.NewLine;       //this.ImportarSQL.ColumnasI[i].ToString()
            RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] rw" + LBTablas.SelectedValue + " = (DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[])dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Select(" + commilla + "Codigo" + LBTablas.SelectedValue + ">0" + commilla + ", " + commilla + this.ImportarSQL.ColumnasI[1].ToString() + commilla + ");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return rw" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //Llenar DTO
            RTBCodigoGenerado.Text += "public void Llenar" + LBTablas.SelectedValue + "()" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = Llenar_DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //borrar registro
            //RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " BorrarDatos" + LBTablas.SelectedValue + "(int Codigo" + LBTablas.SelectedValue + ")" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = ws" + TXTWebservices.Text.ToString() + ".Borrar_" + LBTablas.SelectedValue + "(Codigo" + LBTablas.SelectedValue + ", false);" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return dto" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            #endregion

            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//dav_leo1@hotmail.com" + Environment.NewLine;
            RTBCodigoGenerado.Focus();
            RTBCodigoGenerado.SelectAll();
            #endregion
            #region para borrar los datos de una tabla
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Borrar_" + LBTablas.SelectedValue + "()";
            RTBCodigoGenerado.Text += Environment.NewLine + "{" + Environment.NewLine;

            RTBCodigoGenerado.Text += "string cadena = ConfigurationManager.ConnectionStrings[1].ConnectionString;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + " dto" + LBTablas.SelectedValue + " = new DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlParameter[] parametros;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = SqlHelperParameterCache.GetCachedParameterSet(cadena, " + ComillaSimple + "Borrar_" + LBTablas.SelectedValue + ComillaSimple + ");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "if (parametros == null)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros = new SqlParameter[] {" + Environment.NewLine;
            RTBCodigoGenerado.Text += "new SqlParameter()," + Environment.NewLine;
            RTBCodigoGenerado.Text += "new SqlParameter()" + Environment.NewLine;

            RTBCodigoGenerado.Text += "};" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlHelperParameterCache.CacheParameterSet(cadena, " + @"""" + "Borrar_" + LBTablas.SelectedValue + @"""" + ", parametros);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[0].ParameterName = " + @"""" + "@" + @"""" + "+ dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Codigo" + LBTablas.SelectedValue + "Column.ToString();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[0].SqlDbType = SqlDbType.Int;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[1].ParameterName = " + @"""" + "@" + "FilaEstado" + @"""" + ";" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            RTBCodigoGenerado.Text += "parametros[0].Value = edi" + LBTablas.SelectedValue + "._Codigo" + LBTablas.SelectedValue + ";" + Environment.NewLine;

            RTBCodigoGenerado.Text += "parametros[1].Value = false;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "SqlHelper.ExecuteNonQuery(cadena, CommandType.StoredProcedure, " + @"""" + "PAD_" + LBTablas.SelectedValue + @"""" + ", parametros);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return Llenar_DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine + "}" + Environment.NewLine;
            #endregion
            #region metodos adicionales
            //RTBCodigoGenerado.Text += "public EDI" + LBTablas.SelectedValue + " DevolverEntidad_" + LBTablas.SelectedValue + "()" + Environment.NewLine;            
            //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "EDI" + LBTablas.SelectedValue + " edi" + LBTablas.SelectedValue + " = new EDI" + LBTablas.SelectedValue+ "();" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "return edi" + LBTablas.SelectedValue + ";" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
    //        RTBCodigoGenerado.Text += "#region metodos adicionales" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] LlenarDDL" + LBTablas.SelectedValue + "()" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
    //    RTBCodigoGenerado.Text += "string Descripcion = dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".DescripcionColumn.ToString();" + Environment.NewLine;
    //    RTBCodigoGenerado.Text += "DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[] rw" + LBTablas.SelectedValue + " = (DTO" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + "Row[])dto" + LBTablas.SelectedValue + "." + LBTablas.SelectedValue + ".Select(" + @"""" + "Codigo" + LBTablas.SelectedValue + ">0" + @"""" + ", " + @"""" + "Descripcion" + @"""" + ");" + Environment.NewLine;
    //    RTBCodigoGenerado.Text += "return rw" + LBTablas.SelectedValue + ";" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "public void Llenar" + LBTablas.SelectedValue + "()" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "{" + Environment.NewLine;
    //    RTBCodigoGenerado.Text += "dto" + LBTablas.SelectedValue + " = Llenar_DTO" + LBTablas.SelectedValue + "();" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "}" + Environment.NewLine;
    //RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
    RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion
        }
        public void CrearLosWebMetodos()
        {
            string tipoDato;
            string comilla = @"""";
            RTBCodigoGenerado.Text += Environment.NewLine+"#region " + LBTablas.SelectedValue + Environment.NewLine;
            #region para crear el web metodo llenar DTO
            
            RTBCodigoGenerado.Text += "[WebMethod(Description = " + comilla +"Llenar DTO"+LBTablas.SelectedValue + comilla + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//LLENAR EL DTO" + LBTablas.SelectedValue  + Environment.NewLine;
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue +" Llenar_DTO" + LBTablas.SelectedValue +"()"+ Environment.NewLine;
            RTBCodigoGenerado.Text += "{"  + Environment.NewLine;
            RTBCodigoGenerado.Text += "CSW" +  LBTablas.SelectedValue + " "+LBTablas.SelectedValue + " = new CSW"+LBTablas.SelectedValue +"();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return " +  LBTablas.SelectedValue + "." + "Llenar_DTO" + LBTablas.SelectedValue +"();"+ Environment.NewLine;
            RTBCodigoGenerado.Text += "}"  + Environment.NewLine;
            #endregion                
            #region para crear el web metodo de adicionar un registro            
            RTBCodigoGenerado.Text += "[WebMethod(Description = " + comilla + "Devuelve el codigo de el registro ingresado y Adicionar un " + LBTablas.SelectedValue  + comilla + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//Adicionar un registro de " + LBTablas.SelectedValue + Environment.NewLine;
            RTBCodigoGenerado.Text += "public int Adicionar_" + LBTablas.SelectedValue + "(" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 1)
                    { RTBCodigoGenerado.Text += ","; }
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += tipoDato + " " + this.ImportarSQL.ColumnasI[i];
                }
            }
            RTBCodigoGenerado.Text += ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "CSW" + LBTablas.SelectedValue + " " + LBTablas.SelectedValue + " = new CSW" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return " + LBTablas.SelectedValue + ".Adicionar_" + LBTablas.SelectedValue + "(" + Environment.NewLine;
            for (int i = 1; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 1)
                    { RTBCodigoGenerado.Text += ","; }                    
                    RTBCodigoGenerado.Text += this.ImportarSQL.ColumnasI[i];
                }
            }
            RTBCodigoGenerado.Text += ");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion                
            #region para crear el web metodo de modificar los datos de un registro
            RTBCodigoGenerado.Text += "[WebMethod(Description = " + comilla + "Devuelve el DTO del registro de los  " + LBTablas.SelectedValue + comilla + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//Modificar un registro de " + LBTablas.SelectedValue + Environment.NewLine;
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue + " Modificar_" + LBTablas.SelectedValue + "(" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 0)
                    { RTBCodigoGenerado.Text += ","; }
                    tipoDato = this.ImportarSQL.TypoColumnasI[i].ToString();
                    tipoDato = validarDato(tipoDato);
                    RTBCodigoGenerado.Text += tipoDato + " " + this.ImportarSQL.ColumnasI[i];
                }
            }
            RTBCodigoGenerado.Text += ")" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "CSW" + LBTablas.SelectedValue + " " + LBTablas.SelectedValue + " = new CSW" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return " + LBTablas.SelectedValue + ".Modificar_" + LBTablas.SelectedValue + "(" + Environment.NewLine;
            for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
            {
                if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                {
                    if (i > 0)
                    { RTBCodigoGenerado.Text += ","; }
                    RTBCodigoGenerado.Text +=this.ImportarSQL.ColumnasI[i];
                }
            }
            RTBCodigoGenerado.Text += ");" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion                
            #region para crear el web metodo de borrar el registro de un dato
            RTBCodigoGenerado.Text += "[WebMethod(Description = " + comilla + "Borrado de registros " + LBTablas.SelectedValue + comilla + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//Borrar un registro de " + LBTablas.SelectedValue + Environment.NewLine;
            RTBCodigoGenerado.Text += "public DTO" + LBTablas.SelectedValue   + " Borrar_"+ LBTablas.SelectedValue + "(" ;
            RTBCodigoGenerado.Text += "int Codigo" + LBTablas.SelectedValue + ", bool FilaEstado)" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "CSW" + LBTablas.SelectedValue + " " + LBTablas.SelectedValue + " = new CSW" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return " + LBTablas.SelectedValue + ".Borrar_" + LBTablas.SelectedValue + "(" ;
            RTBCodigoGenerado.Text += "Codigo" + LBTablas.SelectedValue + ", FilaEstado);" + Environment.NewLine;
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion                
            #region para crear el web metodo devolver una clase
            RTBCodigoGenerado.Text += "[WebMethod(Description = " + comilla + "Devolver Clase " + LBTablas.SelectedValue + comilla + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "//devolver la clase un registro de " + LBTablas.SelectedValue + Environment.NewLine;
            RTBCodigoGenerado.Text += "public EDI" + LBTablas.SelectedValue + " DevolverClase_" + LBTablas.SelectedValue + "()";            
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "CSW" + LBTablas.SelectedValue + " " + LBTablas.SelectedValue + " = new CSW" + LBTablas.SelectedValue + "();" + Environment.NewLine;
            RTBCodigoGenerado.Text += "return " + LBTablas.SelectedValue + ".DevolverEntidad_" + LBTablas.SelectedValue + "();";            
            RTBCodigoGenerado.Text += "}" + Environment.NewLine;
            #endregion
            RTBCodigoGenerado.Text += "#endregion" + Environment.NewLine;
        }

        private void guardarArchivoTexto_Click(object sender, EventArgs e)
        {
            string Nombreclase ="";
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
            
            RTBCodigoGenerado.SaveFile(nombre,RichTextBoxStreamType.PlainText);            
                }
                catch
            {
                    MessageBox.Show("No se encontro la ruta para guardar este archivo");
                }
        }

        private void BTGenerarAutomaticamente_Click(object sender, EventArgs e)
        {
            
            #region crear las carpetas
            string directorio = TXTRutaArchivo.Text;
            if (Directory.Exists(directorio))
            {
                directorio = directorio + "\\WS" + BaseDatos.NombreBaseDatos.ToString();
                Directory.CreateDirectory(directorio + "\\App_WebReferences");
                Directory.CreateDirectory(directorio + "\\Bin");
                directorio = directorio + "\\App_Code";                
                Directory.CreateDirectory(directorio);
                Directory.CreateDirectory(directorio + "\\Reutilizables");
                Directory.CreateDirectory(directorio + "\\Entidades");
                Directory.CreateDirectory(directorio + "\\Controladoras");
            }
            else
            {            //Runtimefolder is created         
                MessageBox.Show("Ingrese un directorio valido para guardar el servicio web");
                            
            }
            
            BTNGenerarCodigo.Enabled = true;
            #region            
            for (int i = 0; i < LBTablas.Items.Count; i++)
            {
                LBTablas.SelectedIndex = i;
                
                    BaseDatos.Tablas.Add(LBTablas.SelectedValue.ToString());
                    //CrearDTO(LBTablas.SelectedValue.ToString(), directorio + "\\Reutilizables\\");
                    CrearDTOVacio(LBTablas.SelectedValue.ToString(), directorio + "\\Reutilizables\\");
                
            }
            #endregion
            #endregion
            
            #region cargamos los campo
            BTNGenerarCodigo.Enabled = true;
            //creamos las entidades
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
                    for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    {
                        if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                        {
                            this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                            this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);

                        }
                    }
                    RTBCodigoGenerado.Text = "";
                    CrearLasEntidades();
                    if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                    {
                        RTBCodigoGenerado.SaveFile(directorio + "\\Entidades\\EDI" + ImportarSQL.tablaNombre.ToString() + ".cs", RichTextBoxStreamType.PlainText);
                        RTBCodigoGenerado.SaveFile(directorio + "\\Entidades\\EDI" + ImportarSQL.tablaNombre.ToString() + ".cs", RichTextBoxStreamType.PlainText);
                    }
                
            }
            //creamos las controladoras
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
                    for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    {
                        if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                        {
                            this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                            this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);

                        }
                    }
                    RTBCodigoGenerado.Text = "";
                    CrearLasControladoras();
                    if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                    {
                        RTBCodigoGenerado.SaveFile(directorio + "\\Controladoras\\C" + ImportarSQL.tablaNombre.ToString() + ".cs", RichTextBoxStreamType.PlainText);
                        RTBCodigoGenerado.SaveFile(directorio + "\\Controladoras\\C" + ImportarSQL.tablaNombre.ToString() + ".cs", RichTextBoxStreamType.PlainText);
                    }
                
            }
            //creamos los web metodos
            RTBCodigoGenerado.Text = "using System;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.Services;" + Environment.NewLine;
            RTBCodigoGenerado.Text += "using System.Web.Services.Protocols;" + Environment.NewLine;
            //RTBCodigoGenerado.Text += "using WSSeguridad;" + Environment.NewLine;

            RTBCodigoGenerado.Text += "[WebService(Namespace = " + @"""" + "http://tempuri.org/" + @"""" + ")]" + Environment.NewLine;
            RTBCodigoGenerado.Text += "[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]" + Environment.NewLine;            

            RTBCodigoGenerado.Text += "public class Service : System.Web.Services.WebService" + Environment.NewLine;
            RTBCodigoGenerado.Text += "{" + Environment.NewLine;
            RTBCodigoGenerado.Text += "public Service () {}" + Environment.NewLine;
            

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
                    for (int i = 0; i <= this.ImportarSQL.ColumnasI.Count - 1; i++)
                    {
                        if (this.ImportarSQL.ColumnasI[i].ToString() != "FilaEstado")
                        {
                            this.LBCampos.Items.Add(this.ImportarSQL.ColumnasI[i]);
                            this.LBCampos.Items.Add(this.ImportarSQL.TypoColumnasI[i]);

                        }
                    }
                    if (ImportarSQL.tablaNombre.ToString() != "dtproperties")
                    {
                        CrearLosWebMetodos();
                    }
                
            }
            RTBCodigoGenerado.Text += Environment.NewLine + "}";
            RTBCodigoGenerado.SaveFile(directorio + "\\Service.cs", RichTextBoxStreamType.PlainText);
            #endregion
            MessageBox.Show("Creacion realizada correctamente");
        }
        private string CrearDTO(string nombre,string ruta)
        {            
            //Checks that already folder of this name exists or not

            DataSet ds = new DataSet();
            ds.Tables.Add(nombre);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM " + nombre,Error2.Text.ToString());
            //sda.Fill(ds,nombre);
            sda.FillSchema(ds,SchemaType.Mapped,nombre);
            ds.WriteXmlSchema(ruta + "DTO" + nombre + ".xsd");
            return nombre;
        }
        private string CrearDTOVacio(string nombre, string ruta)
        {   // " + comilla + "  " + nombre + "
            string comilla = @"""";
            RTBCodigoGenerado.Text = "";
            RTBCodigoGenerado.Text += "<?xml version=" + comilla + "1.0" + comilla + " encoding=" + comilla + "utf-8" + comilla + "?>" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<xs:schema id=" + comilla + "DTO" + nombre + "" + comilla + " targetNamespace=" + comilla + "http://tempuri.org/DTO" + nombre + ".xsd" + comilla + " xmlns:mstns=" + comilla + "http://tempuri.org/DTO" + nombre + ".xsd" + comilla + " xmlns=" + comilla + "http://tempuri.org/DTO" + nombre + ".xsd" + comilla + " xmlns:xs=" + comilla + "http://www.w3.org/2001/XMLSchema" + comilla + " xmlns:msdata=" + comilla + "urn:schemas-microsoft-com:xml-msdata" + comilla + " xmlns:msprop=" + comilla + "urn:schemas-microsoft-com:xml-msprop" + comilla + " attributeFormDefault=" + comilla + "qualified" + comilla + " elementFormDefault=" + comilla + "qualified" + comilla + ">" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<xs:annotation>" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<xs:appinfo source=" + comilla + "urn:schemas-microsoft-com:xml-msdatasource" + comilla + ">" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<DataSource DefaultConnectionIndex=" + comilla + "0" + comilla + " FunctionsComponentName=" + comilla + "QueriesTableAdapter" + comilla + " Modifier=" + comilla + "AutoLayout, AnsiClass, Class, Public" + comilla + " SchemaSerializationMode=" + comilla + "IncludeSchema" + comilla + " xmlns=" + comilla + "urn:schemas-microsoft-com:xml-msdatasource" + comilla + ">" + Environment.NewLine;
            RTBCodigoGenerado.Text += "<Connections>" + Environment.NewLine;
            RTBCodigoGenerado.Text += "</Connections>" + Environment.NewLine;    
            RTBCodigoGenerado.Text += "<Tables>"+ Environment.NewLine ;
            RTBCodigoGenerado.Text += "</Tables>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "<Sources>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</Sources>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += ""+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</DataSource>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</xs:appinfo>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</xs:annotation>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "<xs:element name=" + comilla + "DTO" + nombre + "" + comilla + " msdata:IsDataSet=" + comilla + "true" + comilla + " msdata:UseCurrentLocale=" + comilla + "true" + comilla + " msprop:Generator_UserDSName=" + comilla + "DTO" + nombre + "" + comilla + " msprop:Generator_DataSetName=" + comilla + "DTO" + nombre + "" + comilla + ">"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "<xs:complexType>"+ Environment.NewLine ;
            RTBCodigoGenerado.Text += "<xs:choice minOccurs=" + comilla + "0" + comilla + " maxOccurs=" + comilla + "unbounded" + comilla + " />" +Environment.NewLine;    
            RTBCodigoGenerado.Text += ""+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</xs:complexType>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</xs:element>"+ Environment.NewLine ;    
            RTBCodigoGenerado.Text += "</xs:schema>"+ Environment.NewLine ;
            RTBCodigoGenerado.SaveFile(ruta + "DTO" + nombre + ".xsd", RichTextBoxStreamType.PlainText);
            RTBCodigoGenerado.SaveFile(ruta + "DTO" + nombre + ".xsd", RichTextBoxStreamType.PlainText);
            RTBCodigoGenerado.Text = "";
            RTBCodigoGenerado.SaveFile(ruta + "DTO" + nombre + ".xss",RichTextBoxStreamType.PlainText);
            return nombre;
        }
    }
}