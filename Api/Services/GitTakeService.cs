using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Models;
using Api.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Services
{
    public class GitTakeService : IGitTakeService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client = new HttpClient();
        private List<HttpResponseMessage> _listResponse = new List<HttpResponseMessage>();
        private HttpResponseMessage _response = new HttpResponseMessage();
        private List<Root> listaRoot = new List<Root>();
        private List<Root> aux = new List<Root>();

        public GitTakeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<HttpResponseMessage>> ChamarApiGithub(string user)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "request");
            _client.DefaultRequestHeaders.Add("Authorization", _configuration.GetValue<string>("GithubToken"));

            int page = 1;
            do
            {
                var path = ($"https://api.github.com/users/{user}/repos?page={page}&per_page=100");

                _response = await _client.GetAsync(path);

                _listResponse.Add(_response);

                page++;

            } while (!string.IsNullOrEmpty(_response.Content.ReadAsStringAsync().Result) && _response.Content.ReadAsStringAsync().Result != "[]" && _response.StatusCode.Equals(200));

            return _listResponse;
        }

        public async Task<List<GitTakeViewModel>> MontarListaRepos(string user)
        {
            var response = await ChamarApiGithub(user);

            List<GitTakeViewModel> lista = new List<GitTakeViewModel>();
            GitTakeViewModel objeto = new GitTakeViewModel();

            for (int x = 0; x < response.Count; x++)
            {
                if (response[x].IsSuccessStatusCode)
                {
                    string result = response[x].Content.ReadAsStringAsync().Result;

                    listaRoot = JsonConvert.DeserializeObject<List<Root>>(result);

                    listaRoot.ForEach(delegate (Root item)
                    {
                        aux.Add(item);
                    });
                }
            }

            var filtroLista = aux.FindAll(f => f.language == "C#")
                                        .OrderBy(f => f.created_at)
                                        .Take(5)
                                        .ToList();

            for (int i = 0; i < filtroLista.Count; i++)
            {
                objeto = new()
                {
                    avatar_url = filtroLista[i].owner.avatar_url,
                    full_name = filtroLista[i].full_name,
                    description = filtroLista[i].description,
                    created_at = filtroLista[i].created_at
                };

                lista.Add(objeto);
            }

            return lista;
        }

        public async Task<object> MontarObjetoBot(string user)
        {
            IDictionary<string, string> itens = new Dictionary<string, string>();
            var repos = await MontarListaRepos(user);

            for (int i = 0; i < repos.Count; i++)
            {
                itens.Add($"avatar_url_{i}", repos[i].avatar_url);
                itens.Add($"full_name_{i}", repos[i].full_name);
                itens.Add($"description_{i}", repos[i].description);
                itens.Add($"created_at_{i}", repos[i].created_at.ToString());
            }

            var objetoJson = JsonConvert.SerializeObject(itens, Formatting.Indented);

            return objetoJson;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}