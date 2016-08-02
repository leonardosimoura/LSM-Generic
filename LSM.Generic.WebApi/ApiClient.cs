using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LSM.Generic.WebApi
{
    public class ApiClient
    {
        private string AcessToken = "";
        private DateTime TokenExpiraEm = DateTime.Now;
        private string UserName = "";
        private string Password = "";

        public ApiClient()
        {
            this.UserName = "";
            this.Password = "";
            this.AcessToken = "";
            this.TokenExpiraEm = DateTime.Now.AddDays(-10);
        }

        /// <summary>
        /// Initialize with UserName and Password
        /// </summary>
        /// <param name="UserName">UserName for auth</param>
        /// <param name="Password">Password for auth</param>
        public ApiClient(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.AcessToken = "";
            this.TokenExpiraEm = DateTime.Now.AddDays(-10);
        }

        /// <summary>
        /// Change the User and Password, this also reset the token
        /// </summary>
        /// <param name="UserName">UserName for auth</param>
        /// <param name="Password">Password for auth</param>
        public void ChangeUserPassword(string UserName, string Password)
        {
            if (UserName != this.UserName  || Password != this.Password)
            {
                this.UserName = UserName;
                this.Password = Password;
                this.AcessToken = "";
                this.TokenExpiraEm = DateTime.Now.AddDays(-10);
            }
        }
        private class TokenInfo
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        /// <summary>
        /// Create the System.Net.Http.HttpClient already authenticates if necessary
        /// </summary>
        /// <param name="UrlRestService">Url where it hosted the WebAPI</param>
        /// <param name="AuthenticationPath">Path for authentication</param>
        /// <returns>System.Net.Http.HttpClient</returns>
        public async Task<System.Net.Http.HttpClient> GetClientAsync(string UrlRestService, string AuthenticationPath)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.MaxResponseContentBufferSize = 256000;

                if (this.AcessToken != "" && DateTime.Now < this.TokenExpiraEm)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AcessToken);
                }
                else
                {
                    var url = string.Format(UrlRestService + AuthenticationPath);                    
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", UserName),
                        new KeyValuePair<string, string>("password", Password)
                    });

                    var resp = await client.PostAsync(url, content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<TokenInfo>(resp.Content.ReadAsStringAsync().Result);
                        AcessToken = result.access_token;
                        TokenExpiraEm = DateTime.Now.AddSeconds(result.expires_in);
                        return await GetClientAsync(UrlRestService, AuthenticationPath);
                    }
                    else
                    {
                        throw new Exception(resp.ReasonPhrase);
                    }
                }
                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PostAsync passing parameter by json ( StringContent ) and returns the TEntity
        /// </summary>
        /// <typeparam name="TEntity">Type of return</typeparam>
        /// <param name="UrlRestService">Url where it hosted the WebAPI</param>
        /// <param name="UrlPath">Path for method</param>
        /// <param name="AuthenticationPath">Path for authentication</param>
        /// <param name="Parameter">Objecto que sera enviado como parametro</param>
        /// <returns>TEntity</returns>
        public async Task<TEntity> PostAsync<TEntity>(string UrlRestService, string UrlPath, string AuthenticationPath, object Parameter = null)
        {
            try
            {
                using (var client = await this.GetClientAsync(UrlRestService, AuthenticationPath))
                {
                    var url = string.Format(UrlRestService + UrlPath);
                    var json = JsonConvert.SerializeObject(Parameter);
                    var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    var resp = await client.PostAsync(url, content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<TEntity>(resp.Content.ReadAsStringAsync().Result);
                        return result;
                    }
                    else
                    {
                        throw new Exception(resp.ReasonPhrase);
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
