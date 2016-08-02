# LSM-Generic


## Generic.WebApi - Usage

var ApiClient = new RestService.ApiClient();

var myclass = await ApiClient.PostAsync<MyClass>(@"http://localhost/", "api/path", "api/security/token");
