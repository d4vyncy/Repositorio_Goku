using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wfaCopyData.CMysql
{
    class AdminDB
    {
        static MySqlConnection Conex = new MySqlConnection();
        static string serv = "Server=5.181.218.1;";
        static string db = "Database=u898421718_db_src_sc_c;";
        static string usuario = "UID=u898421718_CopiaSeguridad;";
        static string pwd = "Password = S3r3c32021$;";
        //static string serv = "Server=Synerdyne.net;";
        //static string db = "Database=synerd5_David;";
        //static string usuario = "UID=synerd5_David;";
        //static string pwd = "Password=D@v1d123..;";
        string CadenaDeConexion = serv + db + usuario + pwd;
        static MySqlCommand Comando = new MySqlCommand();
        static MySqlDataAdapter Adaptador = new MySqlDataAdapter();

        public void Conectar()
        {
            try
            {
                Conex.ConnectionString = CadenaDeConexion;
                Conex.Open();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Desconectar()
        {
            Conex.Close();
        }
        public void RegistrarDatos(string Tabla, ArrayList Columns, DataTable Datos)
        {
            //string col = ListaColumnas(Columns);
            try
            {
                using (MySqlConnection conn = new MySqlConnection(this.CadenaDeConexion))
                {   
                    conn.Open();
                    for (int i = 0; i < Datos.Rows.Count; i++)
                    {
                        string query = ComanText(Columns, Tabla);
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        foreach (DataColumn dc in Datos.Columns)
                        {
                            object valor = Datos.Rows[i][dc.ColumnName];    
                            cmd.Parameters.Add("?" + dc.ColumnName, validarDato(dc.DataType.ToString())).Value = Datos.Rows[i][dc.ColumnName];
                        }
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (MySqlException ex)
            {
                //MessageBox.Show("Error: {0}", ex.Message);
            }
        }
        string ComanText(ArrayList Columns, string Tabla)
        {
            string c = "INSERT INTO `" + Tabla + "`(";
            string values = ") VALUES(";
            //foreach (string i in Columns)
            //{
            //    c += "`" + i + "`" + ",";
            //    values += "?,";
            //}
            foreach (Object obj in Columns)
            {
                c += "`" + obj.ToString() + "`" + ",";
                values += "?,";
            }
            return c.Substring(0, c.Length - 1) + values.Substring(0, values.Length - 1) + ")";
        }
        string ListaColumnas(DataTable dtTable, DataRow dtRow)
        {
            string c = "";
            foreach (DataColumn dc in dtTable.Columns)
            {
                c = dtRow[dc].ToString();
            }
            return c.Substring(0, c.Length - 1);
        }
        void Parametros(ref MySqlCommand cmd, DataTable dtTable, DataRow dtRow)
        {



        }
        public MySqlDbType validarDato(string dato)
        {
            if (dato == "text")
            { return MySqlDbType.VarChar; }
            if (dato == "varchar")
            { return MySqlDbType.VarChar; }
            if (dato == "numeric")
            { return MySqlDbType.Decimal; }
            if (dato == "Int32")
            { return MySqlDbType.Int32; }
            if (dato == "Decimal")
            { return MySqlDbType.Decimal; }
            if (dato == "Char")
            { return MySqlDbType.VarChar; }
            if (dato == "String")
            { return MySqlDbType.VarChar; }
            if (dato == "Int16")
            { return MySqlDbType.Int16; }
            if (dato == "Double")
            { return MySqlDbType.Double; }
            return MySqlDbType.VarChar;
        }
    }

}
