using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSM.Generic.Repository.DataAnnotation;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections;

namespace LSM.Generic.Repository.SqlServer
{
    public class DbContext<T> : IDisposable where T : class
    {
        public string Connection { get; private set; }
        public bool UseTransaction { get; private set; }
        private SqlTransaction Transaction;
        private SqlConnection Conn;

        public DbContext(string conn)
        {
            this.Connection = conn;
            this.UseTransaction = false;
        }
        public DbContext(string conn, bool useTransaction)
        {
            this.Connection = conn;
            this.UseTransaction = useTransaction;
        }

        private void Commit()
        {
            if (UseTransaction)
            {
                if (Transaction != null)
                {
                    Transaction.Commit();
                    Transaction = null;
                }
            }

            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();

                }
                Conn = null;
            }

        }

        private void Rollback()
        {
            if (UseTransaction)
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction = null;
                }
            }

            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();

                }
                Conn = null;
            }
        }

        private SqlConnection GetConnection()
        {
            if (Conn == null)
            {
                var conn = new SqlConnection(Connection);

                conn.Open();

                if (UseTransaction)
                {
                    InitiateTransaction(conn);
                }

                Conn = conn;
            }
            return Conn;
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            if (Conn == null)
            {
                var conn = new SqlConnection(Connection);

                await conn.OpenAsync();

                if (UseTransaction)
                {
                    InitiateTransaction(conn);
                }
                Conn = conn;
            }
            return Conn;
        }

        private void InitiateTransaction(SqlConnection conn)
        {
            if (UseTransaction)
            {
                if (Transaction == null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        Transaction = conn.BeginTransaction();
                    }
                    else
                    {
                        throw new Exception("Connections is not open.");
                    }
                }
            }
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
                                cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                            }
                        }
                    }
                }

                var conn = GetConnection();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
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
            catch (Exception ex)
            {
                Rollback();
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
                                cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                            }
                        }
                    }
                }

                var conn = await GetConnectionAsync();



                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
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
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();


                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return DtMapper.DataTableToObj<T>(dt);
                }

            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = await GetConnectionAsync();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    var dt = new DataTable();
                    dt.Load(await cmd.ExecuteReaderAsync());
                    return DtMapper.DataTableToObj<T>(dt);
                }

            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();
                {
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = Transaction;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = await GetConnectionAsync();
                {

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = Transaction;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();


                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = await GetConnectionAsync();
                {
                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = Transaction;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Rollback();
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
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = await GetConnectionAsync();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    await cmd.ExecuteNonQueryAsync();
                }

            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }



        public IEnumerable<T> ExecuteProcedureWithListReturn(T obj, string procedure)
        {
            try
            {
                var Type = typeof(T);
                OtherProcedureAttribute OtherProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(OtherProcedureAttribute), false))
                {
                    if (((OtherProcedureAttribute)item).ProcedureName == procedure)
                    {
                        OtherProcAttribute = (OtherProcedureAttribute)item;
                        break;
                    }
                }


                if (OtherProcAttribute == null)
                {
                    throw new Exception("class dont have the procedure " + procedure + " assigned for use");
                }

                var cmd = new SqlCommand(OtherProcAttribute.ProcedureName);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    var ProcParameterAttr = (IEnumerable<ProcedureParameterAttribute>)propert.GetCustomAttributes(typeof(ProcedureParameterAttribute));
                    if (ProcParameterAttr.FirstOrDefault( a => a.ProcedureName == procedure) != null)
                    {
                        
                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();

                using (cmd)
                {
                    var dt = new DataTable();
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    dt.Load(cmd.ExecuteReader());

                    return DtMapper.DataTableToList<T>(dt);

                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }

        public T ExecuteProcedureWithObjReturn(T obj, string procedure)
        {
            try
            {
                var Type = typeof(T);
                OtherProcedureAttribute OtherProcAttribute = null;

                foreach (var item in Type.GetCustomAttributes(typeof(OtherProcedureAttribute), false))
                {
                    if (((OtherProcedureAttribute)item).ProcedureName == procedure)
                    {
                        OtherProcAttribute = (OtherProcedureAttribute)item;
                        break;
                    }
                }


                if (OtherProcAttribute == null)
                {
                    throw new Exception("class dont have the procedure " + procedure + " assigned for use");
                }

                var cmd = new SqlCommand(OtherProcAttribute.ProcedureName);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var Properts = Type.GetProperties();
                foreach (var propert in Properts)
                {
                    var ProcParameterAttr = (IEnumerable<ProcedureParameterAttribute>)propert.GetCustomAttributes(typeof(ProcedureParameterAttribute));
                    if (ProcParameterAttr.FirstOrDefault(a => a.ProcedureName == procedure) != null)
                    {

                        var DtMap = (LSM.Generic.Repository.Attribute.DtMap)propert.GetCustomAttributes(typeof(LSM.Generic.Repository.Attribute.DtMap)).FirstOrDefault();
                        if (DtMap != null)
                        {
                            cmd.Parameters.AddWithValue(DtMap.Column, propert.GetValue(obj));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(propert.Name, propert.GetValue(obj));
                        }
                    }
                }

                var conn = GetConnection();

                using (cmd)
                {
                    var dt = new DataTable();
                    cmd.Connection = conn;
                    cmd.Transaction = Transaction;
                    dt.Load(cmd.ExecuteReader());

                    return DtMapper.DataTableToObj<T>(dt);

                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }

        public void Dispose()
        {
            Connection = "";
            Commit();
        }
    }
}
