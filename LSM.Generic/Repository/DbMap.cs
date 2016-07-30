using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Generic.Repository.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DbMap : System.Attribute
    {
        public string Coluna { get; set; }
        public bool Map = true;


        /// <summary>
        /// Utilize para mapear automaticamente a propriedade apartir do nome da coluna de um datatable / datarow
        /// </summary>
        /// <param name="Nome">Nome da Coluna</param>
        public DbMap(string Coluna)
        {
            this.Coluna = Coluna;
        }

        /// <summary>
        /// Utilize para mapear automaticamente a propriedade apartir do nome da coluna de um datatable / datarow
        /// </summary>
        /// <param name="Nome">Nome da Coluna</param>
        /// <param name="Map">Mapear?</param>
        public DbMap(string Coluna, bool Map)
        {
            this.Coluna = Coluna;
            this.Map = Map;
        }
    }

    // Uso
    //public class Trilha
    //{

    //    [DbMap("IdTrilhaTemplate")]
    //    public Int64 IdTrilhaTemplate_TESTE { get; set; }

    //    [DbMap("IdTrilha")]
    //    publaic Int64 IdTrilha_TESTE { get; set; }

    //    [DbMap("IdOwner")]
    //    public Int64 IdOwner_TESTE { get; set; }

    //    [DbMap("NomeTrilha")]
    //    public string NomeTrilha_TESTE { get; set; }

    //    [DbMap("DescricaoTrilha")]
    //    public string DescricaoTrilha_TESTE { get; set; }

    //    [DbMap("DataCadastro")]
    //    public DateTime DataCadastro_TESTE { get; set; }

    //    [DbMap("DataAlteracao")]
    //    public DateTime DataAlteracao_TESTE { get; set; }

    //    [DbMap("DataRemocao")]
    //    public DateTime DataRemocao_TESTE { get; set; }

    //    [DbMap("DataPrimeiroAcesso")]
    //    public DateTime? DataPrimeiroAcesso_TESTE { get; set; }

    //    [DbMap("DataUltimoAcesso")]
    //    public DateTime? DataUltimoAcesso_TESTE { get; set; }

    //    [DbMap("DataConclusao")]
    //    public DateTime? DataConclusao_TESTE { get; set; }


    //    public bool Ativo
    //    {
    //        get
    //        {
    //            if (DataRemocao_TESTE == null)
    //            {
    //                return true;
    //            }

    //            return false;
    //        }
    //    }

    //}
}
