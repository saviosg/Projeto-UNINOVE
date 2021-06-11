using Microsoft.AspNetCore.Mvc;
using Axulus.Data.Repositorio.Sqlite;
using Axulus.Data.Model;

namespace Axulus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        #region Propriedades

        #endregion


        #region Metodos Publicos

        [HttpGet]
        public IActionResult Get()
        {
            var testeRepo = new TesteRepo();
            var testes = testeRepo.ListarTeste();
            return Ok(testes);
        }

        [HttpGet("{id}/")]
        public IActionResult Get(int id)
        {
            var testeRepo = new TesteRepo();
            var teste = testeRepo.ObterTestePorId(id);

            return Ok(teste);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TesteModel teste)
        {
            var testeRepo = new TesteRepo();

            testeRepo.AdicionarTeste(teste);
            return Ok();

        }

        [HttpPut, Route("atualizarTeste")]
        public IActionResult Put([FromBody] TesteModel teste)
        {
            if (teste == null)
                return BadRequest();
            var testeRepo = new TesteRepo();

            var editTeste = testeRepo.EditarTeste(teste);

            return Ok(editTeste);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var testeRepo = new TesteRepo();
            testeRepo.ExcluirTeste(id);

            return Ok();
        }


        #endregion


        #region Metodos Privados

        #endregion
    }
}
