using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace GeneradorCodigoControladoras.Entidades
{
    public class EImportarSQL
    {
        public ArrayList Colum;
        public ArrayList TypoColum;
        public ArrayList LongitudColumna;
        public string NombreTabla;
        public string NombreTablaAlias;
        public EImportarSQL()
        {
            Colum = new ArrayList();
            TypoColum = new ArrayList();
            LongitudColumna = new ArrayList();
            NombreTabla = string.Empty;
            NombreTablaAlias = string.Empty;
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet TablasSqlServer(string CadenaConexion, string ParametroAcronimo)
        {
            //toma todas las tablas de una base de datos de SQLserver
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            string Sql = "SELECT " + '\n' +
                "TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " + '\n' +
                "WHERE " + '\n' +
                "TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'dtproperties' AND TABLE_NAME<>'sysdiagrams' and TABLE_NAME like '%"+ ParametroAcronimo + "%' " + '\n';
            //Condicion --solo para la bd valorados
            //Sql += "and TABLE_NAME in ('trnIngreso','trnSalida','trnbaja','trndevolucion','trnfaltante','trnfallaimprenta','trnfallaimpresion','trndescargo','trnrequerimiento')";
            //Sql += " and TABLE_NAME<>'trnAperturaCierreCaja' and TABLE_NAME<>'trnTarjetaParaControlDeExistencias'"; // and TABLE_NAME<>'trnUsuarioEntidadOD'";
            SqlDataAdapter MiDataAdapter = new SqlDataAdapter();
            MiConexion.Open();
            SqlCommand MiCommand = new SqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet("Tabla");
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            return ds;
        }
        //string ParametroAcronimo = "seg_tbl";
        public DataSet TablasMySqlServer(string CadenaConexion,string baseDatos,string ParametroAcronimo)
        {
            //toma todas las tablas de una base de datos de SQLserver
            MySqlConnection MiConexion = new MySqlConnection(CadenaConexion);
            string Sql = "SELECT " + '\n' +
                "TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " + '\n' +
                "WHERE " + '\n' +
                "TABLE_NAME<>'information_schema'" +
                " AND TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'dtproperties' AND TABLE_NAME<>'sysdiagrams' " + '\n'
           + " and TABLE_SCHEMA = '" + baseDatos + "'" + "AND TABLE_NAME like '%"+ ParametroAcronimo + "%' " + ";";
            //Condicion --solo para la bd valorados
            //Sql += "and TABLE_NAME in ('trnIngreso','trnSalida','trnbaja','trndevolucion','trnfaltante','trnfallaimprenta','trnfallaimpresion','trndescargo','trnrequerimiento')";
            //Sql += " and TABLE_NAME<>'trnAperturaCierreCaja' and TABLE_NAME<>'trnTarjetaParaControlDeExistencias'"; // and TABLE_NAME<>'trnUsuarioEntidadOD'";
            MySqlDataAdapter MiDataAdapter = new MySqlDataAdapter();
            MiConexion.Open();
            MySqlCommand MiCommand = new MySqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet("Tabla");
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            return ds;
        }
        public void CargarColumnas(string CadenaConexion, string Tabla)
        {
            MySqlConnection MiConexion = new MySqlConnection(CadenaConexion);
            string Sql = "Select * From " + Tabla + " LIMIT 0";
            MySqlDataAdapter MiDataAdapter = new MySqlDataAdapter();
            MiConexion.Open();
            MySqlCommand MiCommand = new MySqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet(Tabla);
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            DataColumn dc = new DataColumn();
            int j = ds.Tables["table"].Columns.Count - 1;

            for (int i = 0; i <= j; i++)
            {
                dc = ds.Tables["table"].Columns[i];
                if ((dc.ColumnName.ToString() != "EstadoRegistro") && (dc.ColumnName.ToString() != "FechaRegistro")
                    && (dc.ColumnName.ToString() != "estadoregistro") && (dc.ColumnName.ToString() != "fecharegistro")
                     && (dc.ColumnName.ToString() != "filaestado") )
                {
                    ColumnasI.Add(dc.ColumnName);
                    TypoColumnasI.Add(dc.DataType.ToString().Substring(7));
                    LongitudColumnasI.Add(dc.MaxLength.ToString());
                }
            }
        }
        public void CargarColumnasMysql(string CadenaConexion, string Tabla)
        {
            MySqlConnection MiConexion = new MySqlConnection(CadenaConexion);
            string Sql = "show columns from " + Tabla;
            MySqlDataAdapter MiDataAdapter = new MySqlDataAdapter();
            MiConexion.Open();
            MySqlCommand MiCommand = new MySqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet(Tabla);
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            //int j = ds.Tables["table"].Columns.Count - 1;
            string columna;
            for (int i = 0; i < ds.Tables["table"].Rows.Count; i++)
            {
                columna = ds.Tables["table"].Rows[i]["Field"].ToString();
                if ((columna != "EstadoRegistro") && (columna != "FechaRegistro")
                    && (columna != "estadoregistro") && (columna != "fecharegistro") && (columna != "filaestado"))
                {
                    ColumnasI.Add(ds.Tables["table"].Rows[i]["Field"].ToString());
                    TypoColumnasI.Add(devuelveDatoMySql(ds.Tables["table"].Rows[i]["Type"].ToString()));

                }
            }
        }
        string devuelveDatoMySql(string Dato)
        {
            if (Dato == "int(11)")
            {
                Dato = "int";
            }
            if (Dato == "bit(1)")
            {
                Dato = "bit";
            }

            return Dato;
        }
        public void CargarColumnasSQL(string CadenaConexion, string Tabla)
        {
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            string Sql = "Select top 1 * From " + Tabla;
            SqlDataAdapter MiDataAdapter = new SqlDataAdapter();
            MiConexion.Open();
            SqlCommand MiCommand = new SqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet(Tabla);
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            DataColumn dc = new DataColumn();
            int j = ds.Tables["table"].Columns.Count - 1;
            EBaseDatos eBaseDatos = new EBaseDatos();
            for (int i = 0; i <= j; i++)
            {
                dc = ds.Tables["table"].Columns[i];
                if ((dc.ColumnName.ToString() != "EstadoRegistro")&&(dc.ColumnName.ToString() != "FechaRegistro")
                    && (dc.ColumnName.ToString() != "estadoregistro") && (dc.ColumnName.ToString() != "fecharegistro")
                    && (dc.ColumnName.ToString() != "filaestado"))
                {
                ColumnasI.Add(dc.ColumnName);
                TypoColumnasI.Add(dc.DataType.ToString().Substring(7));
                }
            }
        }
        public DataSet LlenarDatos(string CadenaConexion, string tabla, ArrayList Columnas)
        {
            string Sql = "SELECT " + Columnas[0].ToString();
            for (int i = 1; i <= Columnas.Count - 1; i++)
            {
                Sql += "," + Columnas[i];
            }
            Sql += " FROM " + tabla;
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            SqlDataAdapter MiDataAdapter = new SqlDataAdapter();
            MiConexion.Open();
            SqlCommand MiCommand = new SqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet(tabla);
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            return ds;
        }

        public ArrayList CargarGroupTabla(string CadenaConexion, string tabla, string Campo, bool Agrupar)
        {
            string Sql;
            ArrayList Datos = new ArrayList();
            if (Agrupar == true)
            {
                Sql = "SELECT " + Campo + " FROM " + tabla + " GROUP BY " + Campo;
            }
            else
            {
                Sql = "SELECT " + Campo + " FROM " + tabla;
            }
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            SqlDataAdapter MiDataAdapter = new SqlDataAdapter();
            MiConexion.Open();
            SqlCommand MiCommand = new SqlCommand(Sql, MiConexion);
            MiCommand.CommandType = CommandType.Text;
            MiDataAdapter.SelectCommand = MiCommand;
            DataSet ds = new DataSet(tabla);
            MiDataAdapter.Fill(ds);
            MiConexion.Close();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Datos.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            return Datos;

        }

        public ArrayList ColumnasI
        {
            get
            { return Colum; }
            set
            { Colum.Add(value); }
        }
        public ArrayList LongitudColumnasI
        {
            get
            { return LongitudColumna; }
            set
            { LongitudColumna.Add(value); }
        }
        public ArrayList TypoColumnasI
        {
            get
            { return TypoColum; }
            set
            { TypoColum.Add(value); }
        }

        public string tablaNombre
        {
            get
            { return NombreTabla; }
            set
            { NombreTabla = value; }
        }

    }
}
