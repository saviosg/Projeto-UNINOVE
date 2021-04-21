using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axulus.Data.Model;
using Axulus.Data.Repositorio;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Axulus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {

        #region Propriedades

        #endregion


        #region Metodos Publicos

        [HttpGet]
        public async Task<IActionResult> GetDepartamento()
        {
            var departamentoRepo = new DepartamentoRepo();
            var departamentos = await departamentoRepo.ListarDepartamentoAsync();

            return Ok(departamentos);
        }

        [HttpGet("{id}/")]
        public async Task<IActionResult> GetDepartamentoPorID(int id)
        {
            var departamentoRepo = new DepartamentoRepo();
            var departamentos = await departamentoRepo.ObterDepartamentoPorIdAsync(id);

            return Ok(departamentos);
        }

        [HttpGet("empresa/{id}/")]
        public async Task<IActionResult> GetDepartamentoEmpresaPorID(int id)
        {
            var departamentoRepo = new DepartamentoRepo();
            var departamentos = await departamentoRepo.ObterDepartamentoEmpresaPorIdAsync(id);

            return Ok(departamentos);
        }

        [HttpGet("categoriadepartamento/{id}/")]
        public async Task<IActionResult> GetCategoriasDepartamentoPorID(int id)
        {
            var catDepartamentoRepo = new CategoriasDepartamentoRepo();
            var catDepartamento = await catDepartamentoRepo.ListarCategoriasPorIdDepartamentoAsync(id);

            return Ok(catDepartamento);
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartamento([FromBody] DepartamentoModel departamento)
        {
            var departamentoRepo = new DepartamentoRepo();

            var departamentoModel = await departamentoRepo.AdicionarDepartamentoAsync(departamento);
            return Ok(departamentoModel);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, [FromBody] DepartamentoModel departamento)
        {
            if (departamento == null)
                return BadRequest();
            var departamentoRepo = new DepartamentoRepo();

            var editDepartamento = await departamentoRepo.EditarDepartamentoAsync(departamento, id);

            return Ok(editDepartamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var departamentoRepo = new DepartamentoRepo();
            await departamentoRepo.ExcluirDepartamentoAsync(id);

            return Ok();
        }

        #endregion


        #region Metodos Privados

        #endregion

    }
}
