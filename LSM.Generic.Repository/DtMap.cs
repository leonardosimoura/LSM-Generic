using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Generic.Repository.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DtMap : System.Attribute
    {
        public string Column { get; set; }
        public bool Map { get; set; }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="map">Map?</param>
        public DtMap(bool map)
        {
            this.Map = map;
        }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="column">Name of column</param>
        public DtMap(string column)
        {
            this.Column = column;
            this.Map = true;
        }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="column">Name of column</param>
        /// <param name="map">Map?</param>
        public DtMap(string column, bool map)
        {
            this.Column = column;
            this.Map = map;
        }
    }
    // Uso   
    //    [DtMap("IdEntidade")]
    //    public int Id { get; set; }
}
