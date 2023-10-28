using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;

namespace GeneradorCodigoControladoras.Entidades
{
    public class EBaseDatos
    {
        public string NombreBaseDatos;
        public string Cuenta;
        public string Password;
        public string NombreServidor;
        public static string CadenaConexion;
        public bool Conectado;
        public ArrayList MyArray;
        public ArrayList Columnas;
        public int val;
        public EBaseDatos()
        {
            NombreBaseDatos = string.Empty;
            Cuenta = string.Empty;
            Password = string.Empty;
            NombreServidor = string.Empty;
            Conectado = false;
            CadenaConexion = string.Empty;
            MyArray = new ArrayList();
        }

        public ArrayList Tablas
        {            
            get
            { return MyArray; }
            set
            { MyArray.Add(value);}
        }
        public string ConnectionString
        {
            get 
            {return CadenaConexion;}
            set
            { CadenaConexion = value; }
        }

        public bool _Conectado
        {
            get
            { return Conectado; }
            set
            { Conectado = value; }
        }

        public string _NombreBaseDatos
        {get
            { return NombreBaseDatos; }
         set
            { NombreBaseDatos = value; }
        }
        public string _Cuenta
        {
            get
            { return Cuenta; }
            set
            { Cuenta = value; }
        }
        public string _Password
        {
            get
            { return Password; }
            set
            { Password = value; }
        }
        public string _NombreServidor
        {
            get
            { return NombreServidor; }
            set
            { NombreServidor = value; }
        }
    }
}
