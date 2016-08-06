using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSM.Generic.Repository.DataAnnotation;

using System.Data;
using System.Reflection;
using Npgsql;

namespace LSM.Generic.Repository.PostgreSql
{
    public class DbContext<T> : IDisposable where T : class
    {
        private string Connection = "";

        public DbContext(string Conn)
        {
            this.Connection = Conn;
        }        

        public IEnumerable<T> GetAll(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new NpgsqlCommand(ProcAttribute.GetAll);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (obj != null)
                {
                    var Properts = Type.GetProperties();
                    foreach (var propert in Properts)
                    {
                        if (propert.GetCustomAttributes(typeof(ProcedureGetAllParameterAttribute)) != null)
                        {
                            var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                            if (DtMap != null)
                            {
                                cmd.Parameters.AddWithValue(DtMap.Coluna, propert.GetValue(obj));

                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                            }
                        }
                    }
                }
                
                using (var conn = new NpgsqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        var dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        return DtMapper.DataTableToList<T>(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetById(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new NpgsqlCommand(ProcAttribute.GetById);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    if (propert.GetCustomAttribute(typeof(ProcedureGetByIdParameterAttribute)) != null)
                    {
                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Coluna, propert.GetValue(obj));

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                using (var conn = new NpgsqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        var dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        return DtMapper.DataTableToObj<T>(dt);
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new NpgsqlCommand(ProcAttribute.Remove);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    if (propert.GetCustomAttribute(typeof(ProcedureRemoveParameterAttribute)) != null)
                    {
                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Coluna, propert.GetValue(obj));

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                using (var conn = new NpgsqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new NpgsqlCommand(ProcAttribute.Update);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    if (propert.GetCustomAttribute(typeof(ProcedureUpdateParameterAttribute)) != null)
                    {
                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Coluna, propert.GetValue(obj));

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                using (var conn = new NpgsqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new NpgsqlCommand(ProcAttribute.Add);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    if (propert.GetCustomAttribute(typeof(ProcedureAddParameterAttribute)) != null)
                    {
                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Coluna, propert.GetValue(obj));

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                using (var conn = new NpgsqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Connection = "";
        }
    }
}
