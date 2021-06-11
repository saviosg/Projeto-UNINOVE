using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Axulus.Data.Repositorio.Sqlite;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Axulus.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        #region Metodos Publicos

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categoriasRepo = new CategoriasRepo();
            var categorias = await categoriasRepo.ListarCategoriasAsync();

            return Ok(categorias);
        }

        #endregion
    }
}
