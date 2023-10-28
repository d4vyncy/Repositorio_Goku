using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace GeneradorCodigoControladoras.Entidades
{
    public class EImportarSQL
    {
        public ArrayList Colum;
        public ArrayList TypoColum;
        public string NombreTabla;
        public EImportarSQL()
        {
            Colum = new ArrayList();
            TypoColum = new ArrayList();
            NombreTabla = string.Empty;
            //
            // TODO: Add constructor logic here
            //
        }


        public DataSet TablasSqlServer(string CadenaConexion)
        {
            //toma todas las tablas de una base de datos de SQLserver
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            string Sql = "SELECT " + '\n' +
                "TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " + '\n' +
                "WHERE " + '\n' +
                "TABLE_TYPE = 'BASE TABLE' " + '\n';
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
        public void CargarColumnas(string CadenaConexion, string Tabla)
        {
            SqlConnection MiConexion = new SqlConnection(CadenaConexion);
            string Sql = "Select * From " + Tabla;
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
                if ((ds.Tables["table"].Columns[i].ToString() != "FilaEstado") && (ds.Tables["table"].Columns[i].ToString() != "FechaRegistro"))
                {
                    dc = ds.Tables["table"].Columns[i];
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
