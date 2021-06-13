const listaUsuario = document.querySelector(".lista-usuario");
const usuarioForm = document.querySelector(".form-usuario");
const enviarForms = document.getElementById("enviarForm");

// Variaveis Formulário
const idEmpresa = document.getElementById("selecionarEmpresa");
const idDepartamento = document.getElementById("selecionarDepartamento");
const nomeUsuario = document.getElementById("nome_usuario");
const cpf = document.getElementById("cpf");
const email = document.getElementById("email");
var sexo = document.getElementById("sexo");
const dataNasc = document.getElementById("data_nas");
const descricaoImg = document.getElementById("nomeImgEmpresa");
const imgEmpresa = document.getElementById("imgEmpresa");

// let dadosUsuario = '';

const exibirEmpresas = (empresas) => {
  let options = '<option value=""></option>';

  for (i = 0; i < empresas.length; i++) {
    options += `<option value="${empresas[i].idEmpresa}">${empresas[i].nomeEmpresa}</option>`;
  }
  document.getElementById("selecionarEmpresa").innerHTML += options;
};

function carregarEmpresaPorID(valor) {
  localStorage.setItem("idEmpresaLS", valor);

  fetch(urlAPI + "/departamento/empresa/" + valor)
    .then((s) => s.json())
    .then((dados) => exibirNomeDepartamentos(dados));
}

async function postUsuario() {
  formatandoDataPost();
  formatandoCPF();
  mostrarImgBase64();

  let pegandoValor = document.getElementById("imgTest").innerHTML;
  let formatandoValor = pegandoValor.split('"');
  let valorFormatado = formatandoValor[1];

  let idEmpresaValue = document.getElementById("selecionarEmpresa").value;
  let idEmpresa = JSON.parse(idEmpresaValue)

  let idDepartamentoValue = document.getElementById("selecionarDepartamento").value;
  let idDepartamento = JSON.parse(idDepartamentoValue);
  
  let sexo = document.querySelector('input[name="sexo"]:checked').value;

    const atualizarUsuario = await fetch(urlAPI + "/usuario", {
      method: "POST",
      body: JSON.stringify({
        idEmpresa: idEmpresa,
        idDepartamento: idDepartamento,
        nome_usuario: nomeUsuario.value,
        cpf: cpf.value,
        email: email.value,
        sexo: sexo,
        dataNasc: dataNasc.value,
        usuarioAtivo: 1,
        image: {
          descricao: descricaoImg.value,
          imageData: valorFormatado,
        },
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    }).then((data) => {
      const postListaEmpresa = [];
      postListaEmpresa.push(data);
      console.log(postListaEmpresa);
      // location.reload();
      redirecionarPaginaLista();
    });
}

function carregarDepartamentoPorID(idDepartamento) {
  localStorage.setItem("idDepartamentoLS", idDepartamento);
}

const exibirNomeDepartamentos = (Departamentos) => {
  var options = '<option value=""></option>';

  for (i = 0; i < Departamentos.length; i++) {
    options += `<option value="${Departamentos[i].idDepartamento}">${Departamentos[i].nomeDepartamento}</option>`;
    console.log(Departamentos[i].idDepartamento, Departamentos[i].nomeDepartamento);
  }
  document.getElementById("selecionarDepartamento").innerHTML = options;
};

fetch(urlAPI + "/empresa")
  .then((s) => s.json())
  .then((dados) => exibirEmpresas(dados));

function formatarData(campoTexto) {
  campoTexto.value = mascaraDataCad(campoTexto.value);
}

function mascaraDataCad(valor) {
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

function checkForm(usuarioForm) {
  event.preventDefault();
  validarForm();
}

function validarEmail(email) {
  var regexValid = /^([a-z]){1,}([a-z0-9._-]){1,}([@]){1}([a-z]){2,}([.]){1}([a-z]){2,}([.]?){1}([a-z]?){2,}$/i;
  console.log("É email valido? Resposta: " + regexValid.test(email));
  if (regexValid.test(email)) {
    console.log("Email válido!");
    return true;
  } else {
    console.log("Email inválido!");
    return false;
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

  if (imgEmpresa.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data atualizacão!");
    imgEmpresa.focus();
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

    fileReader.onload = function (event) {
      var urlImg = event.target.result; // <--- data: base64

      var newImage = document.createElement("img");
      newImage.style = "width: 300px; margin: 10px";
      newImage.src = urlImg;

      document.getElementById("imgTest").innerHTML = newImage.outerHTML;
    };
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
