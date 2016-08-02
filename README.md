# LSM-Generic


## Generic.WebApi - Usage

var ApiClient = new ApiClient("User" ,"Password");

var myclass = await ApiClient.PostAsync< Myclass >(@"http://localhost/", "api/path", "api/security/token");

var client = await ApiClient.GetClientAsync(@"http://localhost/","api/security/token");

## Generic.Repository - Usage

var dt = new DataTable();

List<MyClass> list  =  DtMapper.DataTableToList<MyClass>(dt);

List<MyClass> list  =  DtMapper.DataTableToNullableList<MyClass>(dt);

MyClass obj =  DtMapper.DataTableToObj<MyClass>(dt);

MyClass obj = DtMapper.DataRowToObj<MyClass>(dt.Rows[0]);

## LSM.Generic.Mvc


### MvcMapper.GetListOfSelectListItem

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
