const listaDepartamentos = document.querySelector(".lista-departamentos");
const listaDepartamento = document.querySelector(".lista-departamento");
const editarDepartamento = document.querySelector(".editar-departamento");

let dadosDepartamento = "";
let dadosDepartamentoID = "";

var urlAPI = "https://localhost:44318/departamento";

fetch("https://localhost:44318/departamento")
  .then((s) => s.json())
  .then((dados) => exibirNomeEmpresas(dados));

const exibirNomeEmpresas = (empresas) => {
  var options = '<option value=""></option>';

    for(i=0; i < empresas.length; i++) {
      options += `<option value="${empresas[i].idDepartamento}">${empresas[i].nomeEmpresa}</option>`;
    }
    document.getElementById('selecionarEmpresa').innerHTML = options;
};

function carregarDepartamentoPorID(valor) {
  localStorage.setItem("idDepartamento", valor);
  const urlAPI = "https://localhost:44318/departamento/" + valor;

  fetch(urlAPI)
    .then((s) => s.json())
    .then((dados) => exibirDepartamentosID(dados));
}

const exibirDepartamentosID = (u) => {

  dadosDepartamentoID = "";
  dadosDepartamentoID += `        
        <div>
        <label for="IdEmp">ID da empresa selecionada:</label>
        <input type="text" id="IdEmp" name="IdEmp" value=${u.idEmpresa} disabled>
        </div>
        <br />

        <div>
        <label for="NomeDep">Nome do departamento:</label>
        <input type="text" id="NomeDep" name="NomeDep" value=${u.nomeDepartamento} /> <span id="error-email"></span>
        </div>
        <br />

        <button
        type="submit"
        id="enviarFormDep"
        class="buttonload"
        onclick= "putDepartamento()"
      >
        Editar Departamento
      </button>

      <div id="id01" class="modal">
      <span
        onclick="document.getElementById('id01').style.display='none'"
        class="close"
        title="Close Modal"
        >&times;</span
      >
      <div class="modal-content">
        <div class="container">
          <h1>Confirmação de edição</h1>
          <p>Seu departamento foi atualizado com sucesso!</p>

          <div class="clearfix">
            <button type="button" class="confirmbtn" id="confirmar-cadastro" class="confirmbtn" onclick="redirecionarPaginaListaDepartamento()">Continuar</button>
          </div>
        </div>
      </div>
    </div>

    <div id="id02" class="modal">
      <span
        onclick="document.getElementById('id01').style.display='none'"
        class="close"
        title="Close Modal"
        >&times;</span
      >
      <div class="modal-content">
        <div class="container">
          <h1>Erro de Edição</h1>
          <p>Seu cadastro possui algum erro, verifique os campos. </p>

          <div class="clearfix">
            <button type="button" class="deletebtn" onclick="return document.getElementById('id01').style.display='none'">Voltar</button>
          </div>
        </div>
      </div>
    </div>
        `;
  listaDepartamento.innerHTML = dadosDepartamentoID;
  localStorage.setItem("idEmpresa", u.idEmpresa);
};

async function putDepartamento() {
  debugger;
  event.preventDefault();
  var idDepartamentoLS = window.localStorage.getItem("idDepartamento");
  var idDepartamento = JSON.parse(idDepartamentoLS);
  window.localStorage.removeItem("idDepartamento");

  var idEmpresaLS = window.localStorage.getItem("idEmpresa");
  var idEmpresa = JSON.parse(idEmpresaLS);
  window.localStorage.removeItem("idEmpresa");

  console.log("https://localhost:44318/departamento/" + idDepartamento);
  try {
    const atualizarDepartamento = await fetch(urlAPI + "/" + idDepartamento, {
      method: "PUT",
      body: JSON.stringify({
        idEmpresa: idEmpresa,
        nomeDepartamento: NomeDep.value,
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    }).then((response) => {
      console.log(response.body);
      if (response.ok) {
        return (document.getElementById("id01").style.display = "block");
      } else {
        return (document.getElementById("id02").style.display = "block");
      }
    });
    let result = await response.json();
    return result;
  } catch (err) {}
}

function redirecionarPaginaInicial() {
  location.href = "http://127.0.0.1:5500/index.html";
}

function redirecionarPaginaListaDepartamento() {
  location.href =
    "http://127.0.0.1:5500/Projeto.Web/pages/departamento/lista-departamento.html";
}
