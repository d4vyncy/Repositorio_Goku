using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wfaCopyData.CMysql;

namespace wfaCopyData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Entidades.EBaseDatos BaseDatos = new wfaCopyData.Entidades.EBaseDatos();
        public Entidades.EImportarSQL ImportarSQL = new wfaCopyData.Entidades.EImportarSQL();
        public Entidades.EOrdenar eOrdenar = new wfaCopyData.Entidades.EOrdenar();
        public DataSet dts = new DataSet();
        public string CadenaConexion;
        private void btnCopy_Click(object sender, EventArgs e)
        {
            BaseDatos._Cuenta = "sa";
            //BaseDatos._NombreBaseDatos = TXTBaseDatos1.Text;
            BaseDatos._NombreBaseDatos = "master";
            BaseDatos._NombreServidor = ".";
            BaseDatos._Password = "Univalle";
            string Error2 = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
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
                MessageBox.Show("Conexion Exitosa... Puede continuar");
                BaseDatos._Conectado = true;
            }
            catch
            {
                MessageBox.Show("La conexión ha fallado...");
                BaseDatos.Conectado = false;
            }
        }

        private void btnSeleccionarBase_Click(object sender, EventArgs e)
        {
            //region boton conectar
            BaseDatos._NombreBaseDatos = LBBaseDatos.SelectedValue.ToString();
            BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            string Error2 = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            LBTablas.Items.Clear();
            LBTablas.Refresh();
            try
            {
                DataSet ds = new DataSet("Tabla");
                ds = ImportarSQL.TablasSqlServer(BaseDatos.ConnectionString);
                LBTablas.DataSource = ds.Tables["table"];
                LBTablas.DisplayMember = "TABLE_NAME";
                LBTablas.ValueMember = "TABLE_NAME";
                LBTablas.Refresh();
                MessageBox.Show("Conexion Exitosa... Puede continuar");
                BaseDatos._Conectado = true;
            }
            catch
            {
                MessageBox.Show("La conexión ha fallado...");
                BaseDatos.Conectado = false;
            }
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {

            BaseDatos._NombreBaseDatos = LBBaseDatos.SelectedValue.ToString();
            string Con = BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";

            for (int tab = 0; tab < LBTablas.Items.Count; tab++)
            //for (int tab = 0; tab < 1; tab++)
            {
                LBTablas.SelectedIndex = tab;
                BaseDatos.Tablas.Clear();
                BaseDatos.Tablas.Add(LBTablas.SelectedValue);
                this.ImportarSQL.ColumnasI.Clear();
                this.ImportarSQL.TypoColumnasI.Clear();
                this.ImportarSQL.CargarColumnas(Con, BaseDatos.Tablas[0].ToString());
                DataSet ds = this.ImportarSQL.LlenarDatos(Con, BaseDatos.Tablas[0].ToString(), this.ImportarSQL.ColumnasI);

                AdminDB oAdminDB = new AdminDB();
                oAdminDB.RegistrarDatos(BaseDatos.Tablas[0].ToString(), this.ImportarSQL.ColumnasI, ds.Tables[0]);
            }
            MessageBox.Show("importacion exitosa");
        }



    }
}
