const listaUsuarios = document.querySelector(".lista-usuario");
const empresaForm = document.querySelector(".form-empresa");
const atualizaEmpresaForm = document.querySelector(".att-form-empresa");
const botaoAtualizar = document.getElementById(".cad-atualizado");
const formAtualizar = document.getElementById("atualiza-empresa");
const enviarForms = document.getElementById("enviarForm");

let dadosUsuarios = "";

const exibirUsuarios = (u) => {
  Array.from(u).forEach((usuario) => {
    dadosUsuarios += `
        <tr>
        <td class="idUsuario">${usuario.idUsuario}</td>
        <td class="nomeUsuario" id="nomeUsuario">${usuario.nome_usuario}</td>
        <td class="nomeEmpresa" id="nomeEmpresa">${usuario.nomeEmpresa}</td>
        <td class="nomeDepartamento" id="nomeDepartamento">${usuario.nomeDepartamento}</td>
        <td class="cpf" id="cpf">${mascaraCpf(usuario.cpf)}</td>
        <td class="email" id="email">${usuario.email}</td>
        <td class="sexo" id="sexo">${usuario.sexo}</td>
        <td class="dataNascimento" id="dataNascimento">${mascaraData(usuario.dataNasc)}</td>
        <td>
          <button id="atualiza-empresa" onclick="editarItem(${usuario.idUsuario}, ${usuario.idEmpresa})">Editar</button>
        </td>
        <td>
          <button class="deletebtn" onclick="removeItem(${usuario.idUsuario})">Excluir</button>
        </td>
    </tr>
        `;
  });
  listaUsuarios.innerHTML = dadosUsuarios;
};

// METODO GET

fetch(urlAPI + "/usuario")
  .then((s) => s.json())
  .then((dados) => exibirUsuarios(dados));

function editarItem(usuarioId, empresaId) {
  console.log("Pegando ID do usuario", usuarioId, empresaId);

  localStorage.setItem('IdEmpresaUs', empresaId);
  // localStorage.setItem('Idusuario', usuarioId);

  window.location = "editar-usuario?idUsuario="+ usuarioId;
}

// DELETE
async function removeItem(usuarioId) {
  var url = urlAPI + "/usuario/" + usuarioId;
  console.log(url);
  const usuario = await fetch(url, {
    method: "DELETE",
  })
    .then(() => {location.reload();});
}

function mascaraCpf(valor) {
  // return valor.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "$1.$2.$3/$4-$5");
  return valor
  .replace(/\D/g, '')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1-$2')
  .replace(/(-\d{2})\d+?$/, '$1')
}

function mascaraData(data) {

  let formatarData = data.split("-");
  let dataFormatando = formatarData[2] + "/"  + formatarData[1] + "/" + formatarData[0];
  let dataFormatandos = dataFormatando.split("T00:00:00");
  let dataFormatada = dataFormatandos[0] + dataFormatandos[1];

  data = dataFormatada;
  return data;
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    urlSITE + "/Projeto.Web/pages/empresa/lista-empresa.html";
}

function abrirModalConfirmado() {
  return (document.getElementById("id02").style.display = "block");
}

