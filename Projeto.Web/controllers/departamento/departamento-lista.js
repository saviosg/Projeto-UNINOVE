const listaEmpresa = document.querySelector(".lista-departamento");
const empresaForm = document.querySelector(".form-empresa");
const atualizaEmpresaForm = document.querySelector(".att-form-empresa");
const botaoAtualizar = document.getElementById(".cad-atualizado");
const formAtualizar = document.getElementById("atualiza-empresa");
const enviarForms = document.getElementById("enviarForm");

// Variaveis Formulário
const nomeEmpresa = document.getElementById("nomeEmpresa");
const email = document.getElementById("email");
const cnpj = document.getElementById("cnpj");
const dataCadastro = document.getElementById("dataCadastro");
const dataAtualizacao = document.getElementById("dataAtualizacao");
const urlAPI = "https://localhost:44318/departamento";
let dadosEmpresa = "";

const exibirDepartamentos = (u) => {
  Array.from(u).forEach((lista) => {
    dadosEmpresa += `
        <tr>
        <td class=""></td>
        <td class="IdDep">${lista.idDepartamento}</td>
        <td class="IdEmp">${lista.idEmpresa}</td>
        <td class="NomeDep">${lista.nomeDepartamento}</td>
        <td class="NomeEmp" id="cnpjList">${lista.nomeEmpresa}</td>
        <td>
          <button id="atualiza-empresa" onclick="editarItem(${lista.idDepartamento}, ${lista.idEmpresa})">Editar</button>
        </td>
        <td>
          <button class="deletebtn" onclick="removeItem(${lista.idDepartamento})">Excluir</button>
        </td>
    </tr>
        `;
  });
  listaEmpresa.innerHTML = dadosEmpresa;
};

// METODO GET

fetch(urlAPI)
  .then((s) => s.json())
  .then((dados) => exibirDepartamentos(dados));

function editarItem(departamentoId, empresaId) {
  console.log("Pegando ID do Departamento ", departamentoId, empresaId);

  localStorage.setItem('IdDepartamento', departamentoId);
  localStorage.setItem('IdEmpresaQS', empresaId);

  window.location = "editar-departamento.html?idDepartamento="+ departamentoId;
}

// DELETE
async function removeItem(departamentoId) {
  var url = "https://localhost:44318/departamento/" + departamentoId;
  console.log(url);
  const empresa = await fetch(url, {
    method: "DELETE",
  })
    .then((response) => response.json())
    .then((json) => console.log(json))
    .then(location.reload());
}

function redirecionarPaginaInicial() {
  location.href = "http://127.0.0.1:5500/index.html";
}

function redirecionarPaginaInicia() {
  location.href = "http://127.0.0.1:5500/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    "http://127.0.0.1:5500/Projeto.Web/pages/empresa/lista-empresa.html";
}

function abrirModalConfirmado() {
  return (document.getElementById("id02").style.display = "block");
}

