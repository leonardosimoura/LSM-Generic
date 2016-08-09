# LSM-Generic


## Generic.WebApi - Usage

```C#

var ApiClient = new ApiClient("User" ,"Password");

var myclass = await ApiClient.PostAsync<Myclass>(@"http://localhost/", "api/path", "api/security/token");

var client = await ApiClient.GetClientAsync(@"http://localhost/","api/security/token");

```
## Generic.Repository - Usage

```C#

public class Pessoa
{
    [LSM.Generic.Repository.Attribute.DtMap("IdPessoa")] //Use this if the column name is different with the property , also in Generic.Repository.SqlServer will use this to generate procedure parameter name
    public int Id { get; set; }
    public string Nome { get; set; }
    public string SobreNome { get; set; }
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
}

var dt = new DataTable();

List<Pessoa> list  =  DtMapper.DataTableToList<Pessoa>(dt);

List<Pessoa> list  =  DtMapper.DataTableToNullableList<Pessoa>(dt);

Pessoa obj =  DtMapper.DataTableToObj<Pessoa>(dt);

Pessoa obj = DtMapper.DataRowToObj<Pessoa>(dt.Rows[0]);

```

## Generic.Repository.SqlServer

```C#
        [LSM.Generic.Repository.DataAnnotation.Procedure("GetPessoaById", "GetAllPessoa", "AddPessoa", "UpdatePessoa", "RemovePessoa")]
        public class Pessoa
        {
            [LSM.Generic.Repository.DataAnnotation.ProcedureGetByIdParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureUpdateParameter]
            [LSM.Generic.Repository.DataAnnotation.ProcedureRemoveParameter]
            [LSM.Generic.Repository.Attribute.DtMap("IdPessoa")] //Use this if the column name is different with the property , also in Generic.Repository.SqlServer will use this to generate procedure parameter name
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
        }
        
        string Conexao = "Connection String";

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
```

## LSM.Generic.Mvc


### MvcMapper.GetListOfSelectListItem

``` C#
public class Todo
{

    [Key]
    
    public int Id { get; set; }
    
    [LabelForDropDown]
    
    public string Name { get; set; 
    
    public string Code { get; set; }
    
}


var list = new List<Todo>();

list.Add(new Todo() { Id = 1 , Name = "Todo 1" , Code = "T1" });

list.Add(new Todo() { Id = 2, Name = "Todo 2", Code = "T2" });

list.Add(new Todo() { Id = 3, Name = "Todo 3", Code = "T3" });

list.Add(new Todo() { Id = 4, Name = "Todo 4", Code = "T4" });

list.Add(new Todo() { Id = 5, Name = "Todo 5", Code = "T5" });

var listDropDown = LSM.Generic.Mvc.MvcMapper.GetListOfSelectListItem<Todo>(list);
```
