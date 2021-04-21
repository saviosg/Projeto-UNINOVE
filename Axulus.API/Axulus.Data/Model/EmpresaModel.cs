using Axulus.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Axulus.Model
{
    public class EmpresaModel
    {
        public int idEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string email { get; set; }
        public string cnpj { get; set; }
        public DateTime? dataLiberacao { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? dataAtualizacao { get; set; }
        public ImageModel image { get; set; }
    }
}
