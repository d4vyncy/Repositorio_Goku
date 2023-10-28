using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using wfaCopyData.CMysql;

namespace wfaCopyData
{
    public partial class frmMysqlToMysql : Telerik.WinControls.UI.RadForm
    {
        public frmMysqlToMysql()
        {
            InitializeComponent();
        }
        /***
         * 5.181.218.1
        S3r3c32021$
        u898421718_CopiaSeguridad
        u898421718_db_src_sc_c

         * ***/
        public Entidades.EBaseDatos BaseDatos = new wfaCopyData.Entidades.EBaseDatos();
        public Entidades.EImportarSQL ImportarSQL = new wfaCopyData.Entidades.EImportarSQL();
        public Entidades.EOrdenar eOrdenar = new wfaCopyData.Entidades.EOrdenar();
        public DataSet dts = new DataSet();
        public string CadenaConexion;
        static string serv = "Server=192.168.100.241;";
        static string db = "Database=sereci;";
        static string usuario = "UID=david.serrudo;";
        static string pwd = "Password = Sereci2021;";
        //static string serv = "Server=Synerdyne.net;";
        //static string db = "Database=synerd5_David;";
        //static string usuario = "UID=synerd5_David;";
        //static string pwd = "Password=D@v1d123..;";
        string CadenaDeConexion = serv + db + usuario + pwd;
        private void btnConectar_Click(object sender, EventArgs e)
        {
            BaseDatos._Cuenta = "u898421718_CopiaSeguridad";
            //BaseDatos._NombreBaseDatos = TXTBaseDatos1.Text;
            BaseDatos._NombreBaseDatos = "u898421718_db_src_sc_c";
            BaseDatos._NombreServidor = "5.181.218.1";
            BaseDatos._Password = "S3r3c32021$";
            string Error2 = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            BaseDatos.ConnectionString = "data source=" + BaseDatos.NombreServidor + ";initial catalog=" + BaseDatos.NombreBaseDatos + ";persist security info=False;user id=" + BaseDatos.Cuenta + ";workstation id=" + BaseDatos.NombreServidor + ";password =" + BaseDatos.Password + "; packet size=4096";
            //string col = ListaColumnas(Columns);
            string query = "SHOW FULL TABLES FROM sereci";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(CadenaDeConexion))
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    connection.Close();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LBTablas.Items.Add(dt.Rows[i]["Tables_in_sereci"].ToString());
                    }
                }

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show("Error: {0}", ex.Message);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {

            string query = "";
            try
            {
                for (int tab = 0; tab < LBTablas.Items.Count; tab++)
                {
                    LBTablas.SelectedIndex = tab;
                    if (LBTablas.Items[tab].ToString()== "tramite")
                    {
                        AdminDB oAdminDB = new AdminDB();
                        ArrayList arlistColum = new ArrayList();
                        DataTable dtTabla = Data(LBTablas.Items[tab].ToString());
                        for (int i = 0; i < dtTabla.Columns.Count; i++)
                        {
                            arlistColum.Add(dtTabla.Columns[i].ColumnName);
                        }
                        oAdminDB.RegistrarDatos(dtTabla.TableName+"_cen", arlistColum, dtTabla);
                    }
                }
                MessageBox.Show("importacion exitosa");


            }
            catch (MySqlException ex)
            {
                //MessageBox.Show("Error: {0}", ex.Message);
            }
            MessageBox.Show("...");
        }
        public DataTable Data(string Tabla)
        {
            DataTable dt = new DataTable();
            string query = "select * FROM " + Tabla;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(CadenaDeConexion))
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    dt.Load(cmd.ExecuteReader());
                    connection.Close();
                }

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show("Error: {0}", ex.Message);
            }
            return dt;
        }

    }
}
