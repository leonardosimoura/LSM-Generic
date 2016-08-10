using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Generic.Repository.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class OtherProcedureAttribute:Attribute
    {

        public string ProcedureName { get;private set; }

        public OtherProcedureAttribute(string procedureName)
        {
            ProcedureName = procedureName;
        }
    }
}
