﻿using System;

namespace LSM.Generic.Repository.DataAnnotation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProcedureAttribute : Attribute
    {
        public string GetById { get; set; }

        public string GetAll { get; set; }

        public string Add { get; set; }

        public string Update { get; set; }

        public string Remove { get; set; }


        /// <summary>
        /// Initialize passing the procedures names
        /// </summary>
        /// <param name="GetById">Procedure name for GetById</param>
        /// <param name="GetAll">Procedure name for GetAll</param>
        /// <param name="Add">Procedure name for Add</param>
        /// <param name="Update">Procedure name for Update</param>
        /// <param name="Remove">Procedure name for Removes</param>
        public ProcedureAttribute(string GetById, string GetAll, string Add, string Update, string Remove)
        {
            this.GetById = GetById;
            this.GetAll = GetAll;
            this.Add = Add;
            this.Update = Update;
            this.Remove = Remove;
        }
    }
}
