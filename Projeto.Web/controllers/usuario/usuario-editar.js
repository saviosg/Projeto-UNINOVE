const listaUsuario = document.querySelector(".app");
const editarEmpresa = document.querySelector(".editar-departamento");
const enviarForms = document.getElementById("enviarForm");

// Variaveis Formulário
var idEmpresa = document.getElementById("selecionarEmpresa");
var idDepartamento = document.getElementById("selecionarDepartamento");
var nomeUsuario = document.getElementById("nome_usuario");
var cpf = document.getElementById("cpf");
var email = document.getElementById("email");
var sexo = document.getElementById("sexo");
var dataNasc = document.getElementById("data_nas");
var usuarioAtivo = document.getElementById("usuario");
var idImagemUsuario = document.getElementById("idImagemUsuario");
var descricaoImg = document.getElementById("nomeImgEmpresa");
var imgEmpresa = document.getElementById("imgEmpresa");

let dadosUsuario = "";

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

var idUsuario = queryString("idUsuario");
const urlAPIidUsuario = urlAPI + "/usuario/" + idUsuario;

const exibirUsuario = (usuario) => {
  nomeUsuario.value = `${usuario.nome_usuario}`;
  cpf.value = `${usuario.cpf}`;
  email.value = `${usuario.email}`;
  dataNasc.value = `${usuario.dataNasc}`;
  descricaoImg.value = `${usuario.image.descricao}`;
  idImagemUsuario.value = `${usuario.idImagem}`

  let sexo = document.getElementsByName("sexo");

  for (var i = 0; i < sexo.length; i++) {
    if (sexo[i].value === `${usuario.sexo}`) {
        sexo[i].checked = true;
    } else {

    }
  }

  let usAtivo = `${usuario.usuarioAtivo}`;
  console.log(usAtivo);
  console.log(usuarioAtivo.value);

  if(usuarioAtivo.value === `${usuario.usuarioAtivo}`) {
    usuarioAtivo.checked = true;
  }

  let elementCpf = cpf;
  elementCpf.dispatchEvent(new KeyboardEvent("keyup", { key: "a" }));

  let elementdataL = dataNasc;
  elementdataL.dispatchEvent(new KeyboardEvent("keyup", { key: "b" }));


  var image = new Image();

  image.src = usuario.image.imageData;
  image.width = 300;
  image.height = 300;
  document.getElementById("imgTest").appendChild(image);

};

console.log(usuarioAtivo);
console.log(usuarioAtivo.value);

const exibirNomeDepartamentos = (Departamentos) => {
  var options = '<option value=""></option>';

    var idEmpresaLS = window.localStorage.getItem("IdEmpresaUs");
    var idEmpresa = JSON.parse(idEmpresaLS);

  for (i = 0; i < Departamentos.length; i++) {
    if(Departamentos[i].idEmpresa == idEmpresa){
      options += `<option value="${Departamentos[i].idDepartamento}" selected>${Departamentos[i].nomeDepartamento}</option>`;
    } else {
      options += `<option value="${Departamentos[i].idDepartamento}">${Departamentos[i].nomeDepartamento}</option>`;
    }
  }
  document.getElementById("selecionarDepartamento").innerHTML = options;
};

const requestDepartamentos = async () => {
  var idEmpresaLS = window.localStorage.getItem("IdEmpresaUs");
  var idEmpresa = JSON.parse(idEmpresaLS);

  fetch(urlAPI + "/departamento/empresa/" + idEmpresa)
  .then((s) => s.json())
  .then((dados) => exibirNomeDepartamentos(dados));
}

const exibirEmpresas = (empresas) => {
  let options = '';

  var idEmpresaLS = window.localStorage.getItem("IdEmpresaUs");
  var idEmpresa = JSON.parse(idEmpresaLS);

    for(i=0; i < empresas.length; i++) {
      if(empresas[i].idEmpresa == idEmpresa) {
        options += `<option value="${empresas[i].idEmpresa}" selected>${empresas[i].nomeEmpresa}</option>`;
      } else {
        options += `<option value="${empresas[i].idEmpresa}">${empresas[i].nomeEmpresa}</option>`;
      }
    }
    document.getElementById('selecionarEmpresa').innerHTML += options;
}

const requestEmpresas = async () => {
  fetch(urlAPI + "/empresa")
  .then((s) => s.json())
  .then((dados) => exibirEmpresas(dados));
}

function carregarEmpresaPorID(valor) {
  localStorage.setItem("idEmpresaLS", valor);

  fetch(urlAPI + "/departamento/empresa/" + valor)
    .then((s) => s.json())
    .then((dados) => exibirNomeDepartamentos(dados));
}

fetch(urlAPIidUsuario)
  .then((s) => s.json())
  .then(requestEmpresas())
  .then(requestDepartamentos())
  .then((dados) => exibirUsuario(dados));

async function putUsuario() {
  debugger;
  formatandoDataPost();
  formatandoCPF();
  mostrarImgBase64();
  verificarUsuarioAtivo();

  let pegandoValor = document.getElementById("imgTest").innerHTML;
  let formatandoValor = pegandoValor.split('"');
  let valorFormatado = formatandoValor[1];

  let idEmpresaValue = document.getElementById("selecionarEmpresa").value;
  let idEmpresa = JSON.parse(idEmpresaValue)

  let idDepartamentoValue = document.getElementById("selecionarDepartamento").value;
  let idDepartamento = JSON.parse(idDepartamentoValue); 

  let idImagemValue = document.getElementById("idImagemUsuario").value;
  let idImagem = JSON.parse(idImagemValue);
  
  let usuarioAtivoValue = document.getElementById("usuario").value;
  let usuarioAtiv = JSON.parse(usuarioAtivoValue);

  let sexo = document.querySelector('input[name="sexo"]:checked').value;  

    const atualizarUsuario = await fetch(urlAPI + "/usuario/" + idUsuario, {
      method: "PUT",
      body: JSON.stringify({
        idEmpresa: idEmpresa,
        idDepartamento: idDepartamento,
        idImagem: idImagem,
        nome_usuario: nomeUsuario.value,
        cpf: cpf.value,
        email: email.value,
        sexo: sexo,
        dataNasc: dataNasc.value,
        usuarioAtivo: usuarioAtiv,
        image: {
          descricao: descricaoImg.value,
          imageData: valorFormatado,
        },
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    }).then((data) => {
      console.log(data);
      redirecionarPaginaLista();
    })
    .catch(error => {
      console.log(error);
    })
}

function formatarData(campoTexto) {
  campoTexto.value = mascaraDataCad(campoTexto.value);
}

function mascaraDataCad(valor) {
  let dataEnviada = valor;

  let formatandoDataCad = dataEnviada.split("T");
  let formatarDataCad = formatandoDataCad[0];
  let formatacaoDataCad = formatarDataCad.split("-");
  let dataFormatada =
    formatacaoDataCad[2] + formatacaoDataCad[1] + formatacaoDataCad[0];
  dataEnviada = dataFormatada;
  valor = dataEnviada;

  return valor
    .replace(/\D/g, "")
    .replace(/(\d{2})(\d)/, "$1/$2")
    .replace(/(\d{2})(\d)/, "$1/$2")
    .replace(/(\d{4})(\d)/, "$1");
}

function formatarCampo(campoTexto) {
  campoTexto.value = mascaraCpf(campoTexto.value);
}

function mascaraCpf(valor) {
  // return valor.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "$1.$2.$3/$4-$5");
  return valor
    .replace(/\D/g, "")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1-$2")
    .replace(/(-\d{2})\d+?$/, "$1");
}

function checkForm(empresaForm) {
  event.preventDefault();
  validarForm();
}

function validarEmail(email) {
  var regexValid = /^([a-z]){1,}([a-z0-9._-]){1,}([@]){1}([a-z]){2,}([.]){1}([a-z]){2,}([.]?){1}([a-z]?){2,}$/i;
  if (regexValid.test(email)) {
    return true;
  } else {
    return false;
  }
}

function verificarUsuarioAtivo() {
  if(usuarioAtivo.checked === false) {
    usuarioAtivo.value = 0;
    console.log(usuarioAtivo.value);
  } else {
    
  }
}

function validarForm() {
  this.validarEmail(email.value);

  if (idEmpresa.value == "") {
    alert("Formulário inválido. Nome da empresa não informado!");
    idEmpresa.focus();
    return false;
  }

  if (idDepartamento.value == "") {
    alert("Formulário inválido. Preencha o campo com um CNPJ válido!");
    idDepartamento.focus();
    return false;
  }

  if (nomeUsuario.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data cadastro!");
    nomeUsuario.focus();
    return false;
  }

  if (email.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data cadastro!");
    email.focus();
    return false;
  }

  // if (sexo.value == "") {
  //   alert("Formulário inválido. Preencha o campo com a Data cadastro!");
  //   sexo.focus();
  //   return false;
  // }

  if (dataNasc.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data cadastro!");
    dataNasc.focus();
    return false;
  }

  if (cpf.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data cadastro!");
    cpf.focus();
    return false;
  }

  if (descricaoImg.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data atualizacão!");
    descricaoImg.focus();
    return false;
  }

  if (
    idEmpresa.value != null &&
    idDepartamento.value != null &&
    nomeUsuario.value != null &&
    email.value != null &&
    // sexo.value != null &&
    dataNasc.value != null &&
    cpf.value != null &&
    imgEmpresa.value != null &&
    descricaoImg.value != null
  ) {
    enviarForms.disabled = true;
    enviarForms.value = "Aguarde um momento...";
    return (document.getElementById("id01").style.display = "block");
  }
}

function formatandoDataPost() {
  let dataEnviada = dataNasc.value;

  let formatarDataCad = dataEnviada.split("/");
  let dataCadFormatando =
    formatarDataCad[1] + "/" + formatarDataCad[0] + "/" + formatarDataCad[2];
  let dataCadFormatada = new Date(dataCadFormatando);
  let dataCadString = JSON.stringify(dataCadFormatada).replace(/"/g, "");
  dataEnviada = dataCadString;
  dataNasc.value = dataEnviada;
}

function formatandoCPF() {
  let cpfEnviado = cpf.value;
  cpfEnviado = cpfEnviado.replace(/(\.|\/|\-)/g, "");
  cpf.value = cpfEnviado;
}

function mostrarImgBase64() {
  var arquivoSelecionado = document.getElementById("imgEmpresa").files;
  if (arquivoSelecionado.length > 0) {
    var fileToLoad = arquivoSelecionado[0];

    var fileReader = new FileReader();

    fileReader.onload = function(event) {
      var urlImg = event.target.result; // <--- data: base64

      var newImage = document.createElement('img');
      newImage.style = 'width: 300px; margin: 10px'
      newImage.src = urlImg;

      document.getElementById("imgTest").innerHTML = newImage.outerHTML;
      // console.log("Converted Base64 version is " + document.getElementById("imgTest").innerHTML);
    }
    fileReader.readAsDataURL(fileToLoad);
  }
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    urlSITE + "/Projeto.Web/pages/usuario/lista-usuario.html";
}
