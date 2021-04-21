using Axulus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Axulus.Data.Model
{
    public class TesteModel
    {
        public int id_teste { get; set; }
        public int id_empresa { get; set; }
        public int id_usuario { get; set; }
        public EmpresaModel empresaModel { get; set; }
        public UsuarioModel usuarioModel { get; set; }

    }
}
