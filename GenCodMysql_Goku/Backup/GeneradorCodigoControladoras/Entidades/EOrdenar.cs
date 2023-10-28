using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace GeneradorCodigoControladoras.Entidades
{    
    public class EOrdenar
    {
        public ArrayList Lista;
        public EOrdenar()
        {
            Lista = new ArrayList();
        }
        //metodo para Eliminar array duplicados
        public ArrayList EliminarDobles(ArrayList MyArray)
        {
            string a, b;
            for (int i = 0; i < MyArray.Count - 1; i++)
            {
                a = MyArray[i].ToString().Replace(" ", "");
                a = a.Trim();
                if (a.Replace(" ", "") == "")
                {
                    MyArray[i] = "?";
                    a = "?";
                }
                for (int j = i + 1; j < MyArray.Count; j++)
                {
                    b = MyArray[j].ToString().Replace(" ", "");
                    b = b.Trim();
                    if (a.Replace(" ", "") == b.Replace(" ", ""))
                    {
                        MyArray.Remove(MyArray[i]);
                        j = MyArray.Count + 1;
                        i = -1;
                    }
                }
            }
            return MyArray;
        }
        //metodo para remplazar los valores de una tabla por los originales
        public ArrayList RemplazarValoresArray(ArrayList MyArray, string parametro)
        {
            string a;
            //parametro = parametro.ToString().Replace(" ", "");
            for (int i = 0; i < MyArray.Count - 1; i++)
            {
                a = MyArray[i].ToString().Replace(" ", "");
                a = a.Trim();
                if (a.Replace(" ", "") == parametro.Replace(" ", ""))
                {
                    MyArray[i] = parametro;
                }
            }
            return MyArray;
        }
        //lista para eliminar los espacion de los costados
        public ArrayList EliminarEspExtremos(ArrayList MyArray)
        {
            string a;
            
            for (int i = 0; i < MyArray.Count; i++)
            {
                a = MyArray[i].ToString();
                if ((a.Replace(" ", "") == "") || (a.ToString() == "?"))
                {
                    MyArray[i] = "?";
                    a = "?";                    
                }                
                a = a.Trim();
                MyArray[i] = a.ToString();
            }
            return MyArray;
        }
        public ArrayList CargarLista(ArrayList MyArray)
        {

            return MyArray;
        }
    }
}
