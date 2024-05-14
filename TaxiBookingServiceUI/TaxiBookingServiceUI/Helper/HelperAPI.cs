using TaxiBookingServiceUI;

namespace AssetManagementSystemUI
{
    public class HelperAPI
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:7062");
            return Client;
        }

        public async Task<HttpResponseMessage> GetAsync(string url, int id)
        {
            if(id != 0)
            {
                using (HttpClient client = Initial())
                {
                    HttpResponseMessage res = await client.GetAsync(url + $"/{id}");
                    return res;
                }
            }
            else
            {
                using (HttpClient client = Initial())
                {
                    HttpResponseMessage res = await client.GetAsync(url);
                    return res;
                }
            }
        }

        public HttpResponseMessage PostAsync<TModel>(TModel model, string url, string _token)
        {
            using (HttpClient client = Initial())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                }

                if (model == null)
                {
                    return null;
                }
                var postTask = client.PostAsJsonAsync<TModel>(url, model);
                postTask.Wait();

                return postTask.Result;
            }
        }

        public async Task<HttpResponseMessage> GetByStringAsync(string url, string email, string _token)
        {
            using (HttpClient client = Initial())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                }

                HttpResponseMessage res = await client.GetAsync(url + $"/{email}");
                return res;
            }
        }

        public HttpResponseMessage PutAsync<TModel>(string url, TModel model, int id, string _token)
        {
            using (HttpClient client = Initial())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                }

                if (model != null)
                {
                    var putTask = client.PutAsJsonAsync<TModel>(url+$"/{id}", model);
                    putTask.Wait();

                    return putTask.Result;
                }
                return null;
            }
        }

        public HttpResponseMessage PutByStringAsync<TModel>(string url, TModel model, string email, string _token)
        {
            using (HttpClient client = Initial())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                }

                if (model != null)
                {
                    var putTask = client.PutAsJsonAsync<TModel>(url + $"/{email}", model);
                    putTask.Wait();

                    return putTask.Result;
                }
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, int id, string _token)
        {
            using (HttpClient client = Initial())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);
                }

                HttpResponseMessage res = await client.DeleteAsync(url+$"?id={id}");

                return res;
            }
        }

        public HttpResponseMessage Login(LoginViewModel login, string url)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return null;
            }
            using (HttpClient client = Initial())
            {
                var message = new HttpRequestMessage(HttpMethod.Post, url+$"/{login.Email}/{login.Password}");
                var postTask = client.SendAsync(message);
                postTask.Wait();

                return postTask.Result;
            }
        }

        public async Task<List<TModel>> GetName<TModel>(string url, List<TModel> models)
        {
            var res = await GetAsync(url, 0);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                models = JsonConvert.DeserializeObject<List<TModel>>(result);
                return models;
            }
            else
            {
                return null;
            }
        }
    }
}
