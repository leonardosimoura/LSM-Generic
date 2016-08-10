using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace LSM.Generic.Repository.SqlServer.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [LSM.Generic.Repository.DataAnnotation.Procedure("GetPessoaById", "GetAllPessoa", "AddPessoa", "UpdatePessoa", "RemovePessoa")]
        [LSM.Generic.Repository.DataAnnotation.OtherProcedure("GetAllPessoaTest")]
        [LSM.Generic.Repository.DataAnnotation.OtherProcedure("GetPessoaTestWithParameter")]
        

        public class Pessoa
        {
            
            [LSM.Generic.Repository.DataAnnotation.ProcedureParameter("GetPessoaTestWithParameter")]
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
        string Conexao = @"Server=DESKTOP-8SJ2DID\SQL2016; Initial Catalog=Nuget;  Persist Security Info=true; User ID=Nuget; Password=789632145@";



        [TestMethod]
        public void OtherProcedureCallAndDtMapper()
        {
            try
            {
                using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao))
                {
                    IEnumerable<Pessoa> list = context.ExecuteProcedureWithListReturn(new Pessoa(), "GetAllPessoaTest");

                    Pessoa obj = context.ExecuteProcedureWithObjReturn(new Pessoa() { Id = 5 }, "GetPessoaTestWithParameter");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [TestMethod]
        public void ProcedureCallAndDtMapper()
        {
            try
            {
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
            catch (Exception ex)
            {

                throw ex;
            }

            
        }


        [TestMethod]
        public async Task ProcedureCallAndDtMapperAsync()
        {
            try
            {
                using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao))
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var pessoa = new Pessoa();
                        pessoa.Nome = "Nome Async" + i.ToString();
                        pessoa.SobreNome = "SobreNome Async" + i.ToString();
                        pessoa.DataNascimento = DateTime.Now.AddMonths(i);
                        pessoa.Ativo = true;

                        await context.AddAsync(pessoa);
                    }

                    var retorno = await context.GetByIdAsync(new Pessoa() { Id = 150 });

                    var Lista = await context.GetAllAsync();

                    foreach (var item in Lista.Where(i => i.Id >= 750 && i.Id <= 1000))
                    {
                        item.Nome = item.Nome + " Alterado Async";

                        await context.UpdateAsync(item);
                    }

                    foreach (var item in Lista.Where(i => i.Id >= 900 && i.Id <= 1000))
                    {
                        await context.RemoveAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }


        [TestMethod]
        public void ProcedureCallAndDtMapperWithTransaction()
        {

            try
            {
                using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao, true))
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        [TestMethod]
        public async Task ProcedureCallAndDtMapperWithTransactionAsync()
        {
            try
            {
                using (var context = new LSM.Generic.Repository.SqlServer.DbContext<Pessoa>(Conexao, true))
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var pessoa = new Pessoa();
                        pessoa.Nome = "Nome Async" + i.ToString();
                        pessoa.SobreNome = "SobreNome Async" + i.ToString();
                        pessoa.DataNascimento = DateTime.Now.AddMonths(i);
                        pessoa.Ativo = true;

                        await context.AddAsync(pessoa);
                    }

                    var retorno = await context.GetByIdAsync(new Pessoa() { Id = 150 });

                    var Lista = await context.GetAllAsync();

                    foreach (var item in Lista.Where(i => i.Id >= 750 && i.Id <= 1000))
                    {
                        item.Nome = item.Nome + " Alterado Async";

                        await context.UpdateAsync(item);
                    }

                    foreach (var item in Lista.Where(i => i.Id >= 900 && i.Id <= 1000))
                    {
                        await context.RemoveAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
