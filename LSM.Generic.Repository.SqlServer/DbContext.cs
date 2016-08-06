using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSM.Generic.Repository.DataAnnotation;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace LSM.Generic.Repository.SqlServer
{
    public class DbContext<T> : IDisposable where T : class
    {
        private string Connection = "";

        public DbContext(string Conn)
        {
            this.Connection = Conn;
        }

        public IEnumerable<T> GetAll(T obj = null)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.GetAll);
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

                using (var conn = new SqlConnection(Connection))
                {
                    conn.Open();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        var dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        var lista = new List<T>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var objAdd = DtMapper.DataRowToObj<T>(dt.Rows[i]);
                            var Properts = Type.GetProperties();

                            foreach (var property in Properts)
                            {
                                
                                if (property.PropertyType.IsClass
                                    && !property.PropertyType.FullName.StartsWith("System.")
                                    && property.PropertyType != Type
                                    )
                                {
                                    object propertValue = Activator.CreateInstance(property.PropertyType);

                                    propertValue = DtMapper.DataRowToDynamic(dt.Rows[i], propertValue.GetType());

                                    property.SetValue(objAdd, propertValue);
                                }
                            }

                            lista.Add(objAdd);
                        }
                        return lista;
                        //return DtMapper.DataTableToList<T>(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(T obj = null)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.GetAll);
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

                using (var conn = new SqlConnection(Connection))
                {
                    await conn.OpenAsync();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        var dt = new DataTable();
                        dt.Load(await cmd.ExecuteReaderAsync());

                        var lista = new List<T>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var objAdd = DtMapper.DataRowToObj<T>(dt.Rows[i]);
                            var Properts = Type.GetProperties();

                            foreach (var property in Properts)
                            {

                                if (property.PropertyType.IsClass
                                    && !property.PropertyType.FullName.StartsWith("System.")
                                    && property.PropertyType != Type
                                    )
                                {
                                    object propertValue = Activator.CreateInstance(property.PropertyType);

                                    propertValue = DtMapper.DataRowToDynamic(dt.Rows[i], propertValue.GetType());

                                    property.SetValue(objAdd, propertValue);
                                }
                            }

                            lista.Add(objAdd);
                        }
                        return lista;
                        //return DtMapper.DataTableToList<T>(dt);
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

                var cmd = new SqlCommand(ProcAttribute.GetById);
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

                using (var conn = new SqlConnection(Connection))
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

        public async Task<T> GetByIdAsync(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.GetById);
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

                using (var conn = new SqlConnection(Connection))
                {
                    await conn.OpenAsync();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        var dt = new DataTable();
                        dt.Load(await cmd.ExecuteReaderAsync());
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

                var cmd = new SqlCommand(ProcAttribute.Remove);
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

                using (var conn = new SqlConnection(Connection))
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

        public async Task RemoveAsync(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.Remove);
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

                using (var conn = new SqlConnection(Connection))
                {
                    await conn.OpenAsync();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        await cmd.ExecuteNonQueryAsync();
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

                var cmd = new SqlCommand(ProcAttribute.Update);
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

                using (var conn = new SqlConnection(Connection))
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

        public async Task UpdateAsync(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.Update);
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

                using (var conn = new SqlConnection(Connection))
                {
                    await conn.OpenAsync();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        await cmd.ExecuteNonQueryAsync();
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

                var cmd = new SqlCommand(ProcAttribute.Add);
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

                using (var conn = new SqlConnection(Connection))
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

        public async Task AddAsync(T obj)
        {
            try
            {
                var Type = typeof(T);
                ProcedureAttribute ProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(ProcedureAttribute), false))
                {
                    ProcAttribute = (ProcedureAttribute)item;
                }

                var cmd = new SqlCommand(ProcAttribute.Add);
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

                using (var conn = new SqlConnection(Connection))
                {
                    await conn.OpenAsync();
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        await cmd.ExecuteNonQueryAsync();
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
