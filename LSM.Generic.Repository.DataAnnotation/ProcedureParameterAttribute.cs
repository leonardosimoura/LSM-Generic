using System;

namespace LSM.Generic.Repository.DataAnnotation
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureParameterAttribute : Attribute
    {
        public string ProcedureName { get; private set; }

        public ProcedureParameterAttribute(string procedureName)
        {
            ProcedureName = procedureName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureGetByIdParameterAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureGetAllParameterAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureAddParameterAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureUpdateParameterAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ProcedureRemoveParameterAttribute : Attribute
    {

    }
}
