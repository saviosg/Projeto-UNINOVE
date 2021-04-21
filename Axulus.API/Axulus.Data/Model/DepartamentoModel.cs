using Axulus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Axulus.Data.Model
{
    public class DepartamentoModel
    {
        public int idDepartamento { get; set; }
        public int idEmpresa { get; set; }
        public string nomeDepartamento { get; set; }
        public string nomeEmpresa { get; set; }
        public ICollection<int> categorias { get; set; }
    }
}
