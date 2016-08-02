using LSM.Generic.Repository.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Generic.Repository
{

    /// <summary>
    /// Responsible for Map DataTable or DataRow to objects
    /// </summary>
    public class DtMapper
    {
        /// <summary>
        /// Mapping a DataRow to an object
        /// </summary>
        /// <typeparam name="T">Type/Class of return</typeparam>
        /// <param name="Row">DataRow to be mapped</param>
        /// <returns></returns>
        public static T DataRowToObj<T>(DataRow Row) where T : class
        {
            if (Row.Table.Columns.Count == 0)
            {
                return null;
            }

            //Obtains the type of the generic class
            Type t = typeof(T);

            //Obtains the properties definition of the generic class.
            //With this, we are going to know the property names of the class
            PropertyInfo[] pi = t.GetProperties();

            object defaultInstance = Activator.CreateInstance(t);

            //Create a new instance of the generic class
            //For each property in the properties of the class
            var nomecoluna = "";
            var mapear = true;
            foreach (PropertyInfo prop in pi)
            {
                try
                {
                    nomecoluna = "";
                    mapear = true;
                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(prop);//captura todos os custom Atributes
                    //percorre os atributos
                    foreach (System.Attribute attr in attrs)
                    {
                        if (attr is DtMap)
                        {
                            DtMap a = (DtMap)attr;
                            if (a.Map == true)
                            {
                                nomecoluna = a.Coluna;
                            }
                            else
                            {
                                nomecoluna = "";
                                mapear = false;
                            }
                        }
                    }

                    if (nomecoluna == "")
                    {
                        nomecoluna = prop.Name;
                    }
                    //Verifica se a coluna existe na DataTable
                    if (Row.Table.Columns.Contains(nomecoluna) && mapear == true)
                    {
                        //Get the value of the row according to the field name
                        //Remember that the classïs members and the tableïs field names
                        //must be identical
                        object columnvalue = Row[nomecoluna];
                        //Know check if the value is null. 
                        //If not, it will be added to the instance
                        if (columnvalue != DBNull.Value)
                        {
                            //Set the value dinamically. Now you need to pass as an argument
                            //an instance class of the generic class. This instance has been
                            //created with Activator.CreateInstance(t)
                            //prop.SetValue(defaultInstance, columnvalue, null);
                            try
                            {
                                prop.SetValue(defaultInstance, columnvalue, null);
                            }
                            catch (Exception)
                            {
                                if (prop.PropertyType == typeof(bool))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToBoolean(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToInt32(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToDouble(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(decimal))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToDecimal(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    prop.SetValue(defaultInstance, float.Parse(columnvalue.ToString()), null);
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(prop.Name + ": " + ex.ToString());
                }
            }
            //Now, create a class of the same type of the generic class. 
            //Then a conversion itïs done to set the value
            T myclass = (T)defaultInstance;
            //Add the generic instance to the generic list
            return myclass;
        }

        /// <summary>
        /// Mapping a DataTable to an object (DataTable must contain only 1 DataRow)
        /// </summary>
        /// <typeparam name="T">Type/Class of return</typeparam>
        /// <param name="DataTable">DataTable to be mapped</param>
        /// <returns></returns>
        public static T DataTableToObj<T>(DataTable DataTable) where T : class
        {
            if (DataTable.Rows.Count > 1)
            {
                throw new Exception("DataTable recebida contem mais de 1 registro.");
            }
            if (DataTable.Rows.Count == 0)
            {
                return null;
            }
            //Obtains the type of the generic class
            Type t = typeof(T);
            //Obtains the properties definition of the generic class.
            //With this, we are going to know the property names of the class
            PropertyInfo[] pi = t.GetProperties();
            object defaultInstance = Activator.CreateInstance(t);
            DataRow row = DataTable.Rows[0];
            //Create a new instance of the generic class
            //For each property in the properties of the class
            var nomecoluna = "";
            var mapear = true;
            foreach (PropertyInfo prop in pi)
            {
                try
                {
                    nomecoluna = "";
                    mapear = true;

                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(prop);//captura todos os custom Atributes

                    //percorre os atributos
                    foreach (System.Attribute attr in attrs)
                    {
                        if (attr is DtMap)
                        {
                            DtMap a = (DtMap)attr;
                            if (a.Map == true)
                            {
                                nomecoluna = a.Coluna;
                            }
                            else
                            {
                                nomecoluna = "";
                                mapear = false;
                            }
                        }
                    }
                    if (nomecoluna == "")
                    {
                        nomecoluna = prop.Name;
                    }
                    //Verifica se a coluna existe na DataTable
                    if (DataTable.Columns.Contains(nomecoluna) && mapear == true)
                    {
                        //Get the value of the row according to the field name
                        //Remember that the classïs members and the tableïs field names
                        //must be identical
                        object columnvalue = row[nomecoluna];
                        //Know check if the value is null. 
                        //If not, it will be added to the instance
                        if (columnvalue != DBNull.Value)
                        {
                            //Set the value dinamically. Now you need to pass as an argument
                            //an instance class of the generic class. This instance has been
                            //created with Activator.CreateInstance(t)
                            //prop.SetValue(defaultInstance, columnvalue, null);
                            try
                            {
                                prop.SetValue(defaultInstance, columnvalue, null);
                            }
                            catch (Exception)
                            {
                                if (prop.PropertyType == typeof(bool))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToBoolean(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(int))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToInt32(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(double))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToDouble(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(decimal))
                                {
                                    prop.SetValue(defaultInstance, Convert.ToDecimal(columnvalue), null);
                                }
                                else if (prop.PropertyType == typeof(float))
                                {
                                    prop.SetValue(defaultInstance, float.Parse(columnvalue.ToString()), null);
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(prop.Name + ": " + ex.ToString());
                    //return null;
                }
            }
            //Now, create a class of the same type of the generic class. 
            //Then a conversion itïs done to set the value
            var myclass = (T)defaultInstance;
            //Add the generic instance to the generic list
            return myclass;
        }

        /// <summary>
        /// Mapping a DataTable to an object List, returning an empty List when the DataTable not contain Rows
        /// </summary>
        /// <typeparam name="T">Type/Class of return</typeparam>
        /// <param name="DataTable">DataTable to be mapped</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable DataTable) where T : class
        {
            return PrivateDataTableToList<T>(DataTable, false);
        }

        /// <summary>
        /// Mapping a DataTable for a list of objects, returning null when the DataTable not contain Rows
        /// </summary>
        /// <typeparam name="T">Type/Class of return</typeparam>
        /// <param name="DataTable">DataTable to be mapped</param>
        /// <returns></returns>
        public static List<T> DataTableToNullableList<T>(DataTable DataTable) where T : class
        {
            return PrivateDataTableToList<T>(DataTable, true);
        }

  
        private static List<T> PrivateDataTableToList<T>(DataTable dataTable, bool Nullable = false) where T : class
        {
            if (dataTable.Rows.Count == 0 && Nullable == true)
            {
                return null;
            }
            //This create a new list with the same type of the generic class
            List<T> genericList = new List<T>();

            if (dataTable.Rows.Count == 0 && Nullable == false)
            {
                return genericList;
            }
            //Obtains the type of the generic class
            Type t = typeof(T);
            //Obtains the properties definition of the generic class.
            //With this, we are going to know the property names of the class
            PropertyInfo[] pi = t.GetProperties();
            //For each row in the datatable
            foreach (DataRow row in dataTable.Rows)
            {
                //Create a new instance of the generic class
                object defaultInstance = Activator.CreateInstance(t);
                //For each property in the properties of the class
                var nomecoluna = "";
                var mapear = true;
                foreach (PropertyInfo prop in pi)
                {
                    try
                    {
                        nomecoluna = "";
                        mapear = true;
                        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(prop);//captura todos os custom Atributes
                        //percorre os atributos
                        foreach (System.Attribute attr in attrs)
                        {
                            if (attr is DtMap)
                            {
                                DtMap a = (DtMap)attr;
                                if (a.Map == true)
                                {
                                    nomecoluna = a.Coluna;
                                }
                                else
                                {
                                    nomecoluna = "";
                                    mapear = false;
                                }
                            }
                        }
                        if (nomecoluna == "")
                        {
                            nomecoluna = prop.Name;
                        }
                        //Verifica se a coluna existe na DataTable
                        if (dataTable.Columns.Contains(nomecoluna) && mapear == true)
                        {
                            //Get the value of the row according to the field name
                            //Remember that the classïs members and the tableïs field names
                            //must be identical
                            object columnvalue = row[nomecoluna];
                            //Know check if the value is null. 
                            //If not, it will be added to the instance
                            if (columnvalue != DBNull.Value)
                            {
                                //Set the value dinamically. Now you need to pass as an argument
                                //an instance class of the generic class. This instance has been
                                //created with Activator.CreateInstance(t)
                                //prop.SetValue(defaultInstance, columnvalue, null);
                                try
                                {
                                    prop.SetValue(defaultInstance, columnvalue, null);
                                }
                                catch (Exception)
                                {
                                    if (prop.PropertyType == typeof(bool))
                                    {
                                        prop.SetValue(defaultInstance, Convert.ToBoolean(columnvalue), null);
                                    }
                                    else if (prop.PropertyType == typeof(int))
                                    {
                                        prop.SetValue(defaultInstance, Convert.ToInt32(columnvalue), null);
                                    }
                                    else if (prop.PropertyType == typeof(double))
                                    {
                                        prop.SetValue(defaultInstance, Convert.ToDouble(columnvalue), null);
                                    }
                                    else if (prop.PropertyType == typeof(decimal))
                                    {
                                        prop.SetValue(defaultInstance, Convert.ToDecimal(columnvalue), null);
                                    }
                                    else if (prop.PropertyType == typeof(float))
                                    {
                                        prop.SetValue(defaultInstance, float.Parse(columnvalue.ToString()), null);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(prop.Name + ": " + ex.ToString());
                        //return null;
                    }
                }
                //Now, create a class of the same type of the generic class. 
                //Then a conversion itïs done to set the value
                T myclass = (T)defaultInstance;
                //Add the generic instance to the generic list
                genericList.Add(myclass);
            }
            //At this moment, the generic list contains all de datatable values
            return genericList;
        }
    }
}
