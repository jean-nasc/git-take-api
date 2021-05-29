using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Api.ViewModels;

namespace Api.Interfaces
{
    public interface IGitTakeService : IDisposable
    {
        Task<List<HttpResponseMessage>> ChamarApiGithub(string user);
        Task<List<GitTakeViewModel>> MontarListaRepos(string user);
        Task<object> MontarObjetoBot(string user);
    }
}