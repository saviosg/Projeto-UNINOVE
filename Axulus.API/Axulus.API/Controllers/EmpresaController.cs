using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Axulus.Model;
using Axulus.Data.Repositorio.Sqlite;
using Microsoft.AspNetCore.Cors;

namespace Axulus.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpresaController : ControllerBase
    {

        #region Propriedades

        #endregion


        #region Metodos Publicos

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var empresaRepo = new EmpresaRepo();
            var empresas = await empresaRepo.ListarEmpresaAsync();

            return Ok(empresas);
        }

        [HttpGet("{id}/")]
        public async Task<IActionResult> Get(int id)
        {
            var empresaRepo = new EmpresaRepo();
            var empresas = await empresaRepo.ObterEmpresaPorIdAsync(id);

            return Ok(empresas);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmpresaModel empresa)
        {
            var empresaRepo = new EmpresaRepo();

            var empresaModel = await empresaRepo.AdicionarEmpresaAsync(empresa);
            return Ok(empresaModel);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmpresaModel empresa)
        {
            if (empresa == null)
                return BadRequest();
            var empresaRepo = new EmpresaRepo();

            var editEmpresa = await empresaRepo.EditarEmpresaAsync(empresa, id);

            return Ok(editEmpresa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var empresaRepo = new EmpresaRepo();
            await empresaRepo.ExcluirEmpresaAsync(id);    

            return Ok();
        }

        #endregion


        #region Metodos Privados

        #endregion


    }
}
