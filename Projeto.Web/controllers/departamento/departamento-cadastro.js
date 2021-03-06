const listaEmpresa = document.querySelector(".lista-empresa");
const enviarForms = document.getElementById("enviarFormDep");
const listaEmpresas = document.getElementById("lista-empresas");
const listaDepartamento = document.getElementById("id-selecionado");

let dadosDepartamento = "";
let dadosDepartamentoID = "";

let listaIdsCategoria = [];


// METODO POST

async function postDepartamento() {
  //debugger;
  let idEmpresa = document.getElementById("selectNumber").value;
  let idEmpresaFormatado = parseInt(idEmpresa);


  const atualizarEmpresa = await fetch(urlAPI + "/departamento", {
    method: "POST",
    body: JSON.stringify({
      idEmpresa: idEmpresaFormatado,
      nomeDepartamento: NomeDep.value,
    }),
    headers: {
      "Content-Type": "application/json; charset=UTF-8",
    },
  }).then((data) => { 
    const postListaEmpresa = [];
    postListaEmpresa.push(data);
    console.log(postListaEmpresa);
    //location.reload();
    redirecionarPaginaListaDepartamento();
  });
}

const exibirNomeEmpresas = (empresas) => {
  var options = '<option value=""></option>';

  for (i = 0; i < empresas.length; i++) {
    options += `<option value="${empresas[i].idEmpresa}">${empresas[i].nomeEmpresa}</option>`;
  }
  document.getElementById("selectNumber").innerHTML = options;

  requestCategoriasDepartamento();
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

function requestCategoriasDepartamento() {
  fetch(urlAPI + "/categorias")
    .then((s) => s.json())
    .then((dados) => exibirNomeCategoriasDepartamentos(dados));
}

fetch(urlAPI + "/empresa")
  .then((s) => s.json())
  .then((dados) => exibirNomeEmpresas(dados));

function abrirModalConfirmado() {
  return (document.getElementById("id02").style.display = "block");
}

function checkForm(departamentoForm) {
  event.preventDefault();
  validarForm();
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

  // for(i=0; i < nomescategoria.length; i++) {
  //   console.log("ID SELECIONADO", nomescategoria[i].checked);
  //   console.log(nomescategoria[i].value);
  //   if(nomescategoria[i].checked) {
  //     listaIdsCategoria.push(nomescategoria[i].value);
  //   }
  // }
}

function validarForm() {
  let IdEmp = document.getElementById("selectNumber");

  if (IdEmp.value == "") {
    alert("Formul??rio inv??lido. ID da empresa n??o informado!");
    IdEmp.focus();
    return false;
  }

  if (NomeDep.value == "") {
    alert("Formul??rio inv??lido. Preencha o campo com um Departamento.");
    NomeDep.focus();
    return false;
  }

  if (IdEmp.value != null && NomeDep.value != null) {
    enviarForms.disabled = true;
    enviarForms.value = "Aguarde um momento...";
    return (document.getElementById("id01").style.display = "block");
  }
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaInicia() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaListaDepartamento() {
  location.href =
    urlSITE + "/Projeto.Web/pages/departamento/lista-departamento.html";
}
