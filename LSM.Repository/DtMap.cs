using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Repository.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DtMap : System.Attribute
    {
        public string Coluna { get; set; }
        public bool Map { get; set; }
        /// <summary>
        /// Utilize para mapear automaticamente a propriedade apartir do nome da coluna de um datatable / datarow
        /// </summary>
        /// <param name="Map">Mapear?</param>
        public DtMap(bool Map)
        {
            this.Map = Map;
        }
        /// <summary>
        /// Utilize para mapear automaticamente a propriedade apartir do nome da coluna de um datatable / datarow
        /// </summary>
        /// <param name="Nome">Nome da Coluna</param>
        public DtMap(string Coluna)
        {
            this.Coluna = Coluna;
            this.Map = true;
        }
        /// <summary>
        /// Utilize para mapear automaticamente a propriedade apartir do nome da coluna de um datatable / datarow
        /// </summary>
        /// <param name="Nome">Nome da Coluna</param>
        /// <param name="Map">Mapear?</param>
        public DtMap(string Coluna, bool Map)
        {
            this.Coluna = Coluna;
            this.Map = Map;
        }
    }
    // Uso   
    //    [DtMap("IdTrilhaTemplate")]
    //    public Int64 IdTrilhaTemplate_TESTE { get; set; }
}
