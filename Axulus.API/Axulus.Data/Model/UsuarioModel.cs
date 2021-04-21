using System;
using System.Collections.Generic;
using System.Text;
using Axulus.Model;

namespace Axulus.Data.Model
{
    public class UsuarioModel
    {
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idDepartamento { get; set; }
        public int idImagem { get; set; }
        public string nome_usuario { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string sexo { get; set; }
        public string nomeEmpresa { get; set; }
        public string nomeDepartamento { get; set; }
        public DateTime? dataNasc { get; set; }
        public int usuarioAtivo { get; set; }
        public ImageModel image { get; set; }
    }
}
