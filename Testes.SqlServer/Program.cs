using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes.SqlServer
{
    class Program
    {
        [LSM.Generic.Repository.DataAnnotation.Procedure("GetPessoaById", "GetAllPessoa", "AddPessoa", "UpdatePessoa", "RemovePessoa")]
        public class Pessoa
        {
            [LSM.Generic.Repository.DataAnnotation.ProcedureGetByIdParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureUpdateParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureRemoveParameter]
            [LSM.Generic.Repository.Attribute.DtMap("IdPessoa")]
            public int Id { get; set; }
            [LSM.Generic.Repository.DataAnnotation.ProcedureAddParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureUpdateParameter]
            public string Nome { get; set; }
            [LSM.Generic.Repository.DataAnnotation.ProcedureAddParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureUpdateParameter]
            public string SobreNome { get; set; }
            [LSM.Generic.Repository.DataAnnotation.ProcedureAddParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureUpdateParameter]
            public DateTime DataNascimento { get; set; }

            public bool Ativo { get; set; }

            public PessoaFisica PessoaFisica { get; set; }
        }

        public class PessoaFisica
        {
            public string Nome { get; set; }

            public DateTime DataNascimento { get; set; }

        }

        static void Main(string[] args)
        {
            Teste();
            TesteAsync();

        }

        static void Teste()
        {
            string Conexao = @"Server=DESKTOP-8SJ2DID\SQL2016; Initial Catalog=Nuget;  Persist Security Info=true; User ID=LeonardoMoura; Password=Glicemic070073";

            using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var pessoa = new Pessoa();
                    pessoa.Nome = "Nome " + i.ToString();
                    pessoa.SobreNome = "SobreNome " + i.ToString();
                    pessoa.DataNascimento = DateTime.Now.AddMonths(i);
                    pessoa.Ativo = true;

                    context.Add(pessoa);
                }

                var retorno = context.GetById(new Pessoa() { Id = 150 });

                var Lista = context.GetAll();

                foreach (var item in Lista.Where(i => i.Id <= 250))
                {
                    item.Nome = item.Nome + " Alterado";

                    context.Update(item);
                }

                foreach (var item in Lista.Where(i => i.Id <= 10))
                {
                    context.Remove(item);
                }
            }
        }

        public static async Task TesteAsync()
        {
            string Conexao = @"Server=DESKTOP-8SJ2DID\SQL2016; Initial Catalog=Nuget;  Persist Security Info=true; User ID=LeonardoMoura; Password=Glicemic070073";

            using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao))
            {
                for (int i = 0; i < 1000; i++)
                {
                    var pessoa = new Pessoa();
                    pessoa.Nome = "Nome " + i.ToString();
                    pessoa.SobreNome = "SobreNome " + i.ToString();
                    pessoa.DataNascimento = DateTime.Now.AddMonths(i);
                    pessoa.Ativo = true;

                    await context.AddAsync(pessoa);
                }

                var retorno = await context.GetByIdAsync(new Pessoa() { Id = 150 });

                var Lista = await context.GetAllAsync();

                foreach (var item in Lista.Where(i => i.Id <= 250))
                {
                    item.Nome = item.Nome + " Alterado";

                    await context.UpdateAsync(item);
                }

                foreach (var item in Lista.Where(i => i.Id <= 10))
                {
                   await  context.RemoveAsync(item);
                }
            }
        }
    }
}
