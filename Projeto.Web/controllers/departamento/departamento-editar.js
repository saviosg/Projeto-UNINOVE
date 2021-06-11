const listaDepartamento = document.querySelector(".app");
const editarDepartamento = document.querySelector(".editar-departamento");

let dadosDepartamento = "";
let listaIdsCategoria = [];

// função pra ler querystring
function queryString(parameter) {
  var loc = location.search.substring(1, location.search.length);
  var param_value = false;
  var params = loc.split("&");
  for (i = 0; i < params.length; i++) {
    param_name = params[i].substring(0, params[i].indexOf("="));
    if (param_name == parameter) {
      param_value = params[i].substring(params[i].indexOf("=") + 1);
    }
  }
  if (param_value) {
    return param_value;
  } else {
    return undefined;
  }
}

var idDepartamento = queryString("idDepartamento");
const urlAPIDepartamento = urlAPI + "/" + idDepartamento;

const exibirCategoriasDepartamentosPorID = (catDepartamentoID) => {
  debugger;
  var nomescategoria = document.getElementsByName("idCategoriasDepartamento");

  for (i = 0; i < catDepartamentoID.length; i++) {
    var listaIdCategorias = catDepartamentoID[i].idCategoria;
    console.log(listaIdCategorias);
    for (var i = 0; i < nomescategoria.length; i++) {
      console.log(nomescategoria[i].value);
      if (catDepartamentoID[i].idCategoria == nomescategoria[i].value) {
        nomescategoria[i].checked = true;
      }
    }
  }

  // for (var i = 0; i < nomescategoria.length; i++) {
  //   console.log(nomescategoria[i].value);
  //   if (catDepartamentoID[i].idCategoria == nomescategoria[i].value) {
  //     nomescategoria[i].checked = true;
  //   }
  // }
};

const requestCategoriasDepartamento = async () => {
  fetch(
    urlAPI + "/departamento/categoriadepartamento/" +
      idDepartamento
  )
    .then((s) => s.json())
    .then((dados) => exibirCategoriasDepartamentosPorID(dados));
};

const exibirNomeCategoriasDepartamentos = (catDepartamento) => {
  var options = "";

  for (i = 0; i < catDepartamento.length; i++) {
    options += `<input type="checkbox" id="idCategoriasDepartamento" name="idCategoriasDepartamento" value="${catDepartamento[i].idCategoria}"/>
                <label id="nomeCategoriasDepartamento" name="nomeCategoriasDepartamento"> ${catDepartamento[i].nomeCategoria} </label> <br/>`;
  }
  document.getElementById("categoriasDepartamento").innerHTML = options;
  console.log(catDepartamento);
};

const requestCategorias = async () => {
  fetch(urlAPI + "/categorias")
    .then((s) => s.json())
    .then((dados) => exibirNomeCategoriasDepartamentos(dados));
};

const exibirDepartamentos = (departamento) => {
  // let options = `<option value="${departamento.idEmpresa}">${departamento.nomeEmpresa}</option>`;

  // document.getElementById('selectNumber').innerHTML += options;

  document.getElementById("NomeDep").value = departamento.nomeDepartamento;
  document.getElementById("NomeDep").innerHTML = departamento.nomeDepartamento;
};

const requestDepartamentos = async () => {
  fetch(urlAPIDepartamento)
    .then((s) => s.json())
    .then((dados) => exibirDepartamentos(dados));
};

const exibirEmpresas = (empresas) => {
  let options = "";

  var idEmpresaLS = window.localStorage.getItem("IdEmpresaQS");
  var idEmpresa = JSON.parse(idEmpresaLS);
  console.log(idEmpresa);
  window.localStorage.removeItem("idEmpresaQS");

  for (i = 0; i < empresas.length; i++) {
    if (empresas[i].idEmpresa == idEmpresa) {
      options += `<option value="${empresas[i].idEmpresa}" selected>${empresas[i].nomeEmpresa}</option>`;
    } else {
      options += `<option value="${empresas[i].idEmpresa}">${empresas[i].nomeEmpresa}</option>`;
    }
  }
  document.getElementById("selectNumber").innerHTML += options;
};

const requestEmpresas = async () => {
  fetch(urlAPI + "/empresa")
    .then((s) => s.json())
    .then((dados) => exibirEmpresas(dados));
};

fetch(urlAPIDepartamento)
  .then(requestEmpresas())
  .then(requestDepartamentos())
  .then(requestCategorias())
  .then(requestCategoriasDepartamento());

// METODO PUT

async function putDepartamento() {
  debugger;
  event.preventDefault();
  var idEmpresaLS = window.localStorage.getItem("IdEmpresaQS");
  var idEmpresa = JSON.parse(idEmpresaLS);
  window.localStorage.removeItem("idEmpresa");
  console.log(idEmpresa, "teste id formatado");

  mostrarValorCategorias();
  console.log(listaIdsCategoria, "Chamando array dentro do post");

  try {
    const atualizarDepartamento = await fetch(urlAPIDepartamento, {
      method: "PUT",
      body: JSON.stringify({
        idEmpresa: idEmpresa,
        nomeDepartamento: NomeDep.value,
        categorias: 
        listaIdsCategoria
      ,
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

function carregarDepartamentoPorID(idEmpresa) {
  console.log(idEmpresa);

  localStorage.setItem("idEmpresa", idEmpresa);
}

function mostrarValorCategorias() {
  var nomescategoria = document.getElementsByName("idCategoriasDepartamento");
  // let i = 0

  for (var i = 0; i < nomescategoria.length; i++) {
    if (nomescategoria[i].checked) {
      if (nomescategoria[i].value == "1") {
        var primeiroValor = 1;
        console.log(primeiroValor);
        listaIdsCategoria.push(primeiroValor);
      } else if (nomescategoria[i].value == "2") {
        var segundoValor = 2;
        console.log(segundoValor);
        listaIdsCategoria.push(segundoValor);
      } else if (nomescategoria[i].value == "3") {
        var terceiroValor = 3;
        console.log(terceiroValor);
        listaIdsCategoria.push(terceiroValor);
      } else if (nomescategoria[i].value == "4") {
        var quartoValor = 4;
        console.log(quartoValor);
        listaIdsCategoria.push(quartoValor);
      } else if (nomescategoria[i].value == "5") {
        var quintoValor = 5;
        console.log(quintoValor);
        listaIdsCategoria.push(quintoValor);
      } else if (nomescategoria[i].value == "6") {
        var sextoValor = 6;
        console.log(sextoValor);
        listaIdsCategoria.push(sextoValor);
      }
    }
  }
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    urlSITE + "/Projeto.Web/pages/departamento/lista-departamento.html";
}
