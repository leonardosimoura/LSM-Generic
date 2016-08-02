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
        public string Coluna { get; set; }
        public bool Map { get; set; }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="Map">Map?</param>
        public DtMap(bool Map)
        {
            this.Map = Map;
        }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="Nome">Name of column</param>
        public DtMap(string Coluna)
        {
            this.Coluna = Coluna;
            this.Map = true;
        }

        /// <summary>
        /// Use to automatically map the property from the name of the column of a datatable / datarow
        /// </summary>
        /// <param name="Nome">Name of column</param>
        /// <param name="Map">Map?</param>
        public DtMap(string Coluna, bool Map)
        {
            this.Coluna = Coluna;
            this.Map = Map;
        }
    }
    // Uso   
    //    [DtMap("IdEntidade")]
    //    public int Id { get; set; }
}
