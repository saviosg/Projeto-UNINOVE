using Axulus.Data.Model;
using Axulus.Data.Repositorio.Sqlite;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Axulus.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        #region Propriedades

        #endregion


        #region Metodos Publicos

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarioRepo = new UsuarioRepo();
            var usuarios = await usuarioRepo.ListarUsuarioAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id}/")]
        public async Task<IActionResult> Get(int id)
        {
            var usuarioRepo = new UsuarioRepo();
            var usuario = await usuarioRepo.ObterUsuarioPorIdAsync(id);

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioModel usuario)
        {
            var usuarioRepo = new UsuarioRepo();
            var usuarioModel = await usuarioRepo.AdicionarUsuarioAsync(usuario);

            return Ok(usuarioModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioModel usuario)
        {
            if (usuario == null)
                return BadRequest();
            var usuarioRepo = new UsuarioRepo();

            var editUsuario = await usuarioRepo.EditarUsuarioAsync(usuario, id);

            return Ok(editUsuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioRepo = new UsuarioRepo();
            await usuarioRepo.ExcluirUsuarioAsync(id);

            return Ok();
        }
        #endregion
    }
}
