using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LSM.WebApi
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
        public ApiClient(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.AcessToken = "";
            this.TokenExpiraEm = DateTime.Now.AddDays(-10);
        }
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
        /// Monta o client e já faz a autenticação se necessário
        /// </summary>
        /// <param name="CaminhoRestService">Dominio onde esta hospedado o WebApi</param>
        /// <param name="AuthenticationPath">Caminho para autenticação</param>
        /// <returns>System.Net.Http.HttpClient</returns>
        public async Task<System.Net.Http.HttpClient> GetClient(string CaminhoRestService, string AuthenticationPath)
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
                    var url = string.Format(CaminhoRestService + AuthenticationPath);                    
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
                        return await GetClient(CaminhoRestService, AuthenticationPath);
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
        /// PostAsync passando o Parametro por json ( StringContent ) e retornando Entidade
        /// </summary>
        /// <typeparam name="Entidade">Type a ser retornando pela chamada</typeparam>
        /// <param name="CaminhoRestService">Dominio onde esta hospedado o WebApi</param>
        /// <param name="UrlPath">Caminho do método</param>
        /// <param name="AuthenticationPath">Caminho para autenticação</param>
        /// <param name="Parametro">Objecto que sera enviado como parametro</param>
        /// <returns>Entidade passada</returns>
        public  async Task<Entidade> PostAsync<Entidade>(string CaminhoRestService, string UrlPath, string AuthenticationPath, object Parametro = null)
        {
            try
            {
                using (var client = await this.GetClient(CaminhoRestService, AuthenticationPath))
                {
                    var url = string.Format(CaminhoRestService + UrlPath);
                    var json = JsonConvert.SerializeObject(Parametro);
                    var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                    var resp = await client.PostAsync(url, content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<Entidade>(resp.Content.ReadAsStringAsync().Result);
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
