using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitTakeController : ControllerBase
    {
        private readonly IGitTakeService _gitTakeService;

        public GitTakeController(IGitTakeService gitTakeService)
        {
            _gitTakeService = gitTakeService;
        }

        #region "Infos Swagger"
        /// <summary>
        /// Buscar repositórios na API do Github baseados no usuário informado e retornar as informações dos 5 mais antigos de linguagem C#.
        /// </summary>
        /// <remarks>
        /// Não é possível retornar as informações dos repositórios sem informar o usuário.
        /// </remarks>
        /// <param name="user"> Recebe o nome de usuário do github. </param>
        /// <response code="200"> Retorna informações sobre os repositórios. </response>
        /// <response code="204"> Caso não existam repositórios C# ou o usuário informado não exista. </response>
        #endregion

        [HttpGet("{user}")]
        public async Task<ActionResult<List<GitTakeViewModel>>> GetRepos(string user)
        {
            var repos = await _gitTakeService.MontarListaRepos(user);

            if(repos == null) return NoContent();

            return Ok(repos);
        }


        #region "Infos Swagger"
        /// <summary>
        /// Buscar repositórios na API do Github baseados no usuário informado e retornar as informações dos 5 mais antigos de linguagem C#.
        /// </summary>
        /// <remarks>
        /// Não é possível retornar as informações dos repositórios sem informar o usuário.
        /// </remarks>
        /// <param name="user"> Recebe o nome de usuário do github. </param>
        /// <response code="200"> Retorna informações sobre os repositórios. </response>
        /// <response code="204"> Caso não existam repositórios C# ou o usuário informado não exista. </response>
        #endregion

        [HttpGet("bot/{user}")]
        public async Task<ActionResult<object>> GetReposBot(string user)
        {
            var bot = await _gitTakeService.MontarObjetoBot(user);

            if(bot == null) return NoContent();

            return Ok(bot);
        }
    }
}