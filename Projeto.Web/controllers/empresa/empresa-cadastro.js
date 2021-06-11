const listaEmpresa = document.querySelector(".lista-empresa");
const empresaForm = document.querySelector(".form-empresa");
const atualizaEmpresaForm = document.querySelector(".att-form-empresa");
const botaoAtualizar = document.getElementById(".cad-atualizado");
const formAtualizar = document.getElementById("atualiza-empresa");
const enviarForms = document.getElementById("enviarForm");
let files = document.querySelector("imgEmpresa");

// Variaveis Formulário
const nomeEmpresa = document.getElementById("nomeEmpresa");
const email = document.getElementById("email");
const cnpj = document.getElementById("cnpj");
const dataLiberacao = document.getElementById("dataLiberacao");
const descricaoImg = document.getElementById("nomeImgEmpresa");
const imgEmpresa = document.getElementById("imgEmpresa");

// METODO POST

async function postEmpresa() {
  let pegandoValor = document.getElementById("imgTest").innerHTML;
  let formatandoValor = pegandoValor.split('"');
  let valorFormatado = formatandoValor[1];

  const atualizarEmpresa = await fetch(
    urlAPI + "/empresa/",
    {
      method: "POST",
      body: JSON.stringify({
        nomeEmpresa: nomeEmpresa.value,
        email: email.value,
        cnpj: cnpj.value,
        dataLiberacao: dataLiberacao.value,
        // dataCadastro: dataCadastro.value,
        // dataAtualizacao: dataAtualizacao.value,
        image: {
          descricao: descricaoImg.value,
          imageData: valorFormatado,
        }
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    }
  )
  .then((data) => {
    const postListaEmpresa = [];
    postListaEmpresa.push(data);
    console.log(postListaEmpresa);
    location.reload();
    redirecionarPaginaLista();
  });
  console.log(atualizarEmpresa);
}

// empresaForm.addEventListener("submit", (e) => {
//   e.preventDefault();
//   fetch(urlAPI, {
//     method: "POST",
//     headers: {
//       "Content-type": "application/json",
//     },
//     body: JSON.stringify({
//       nomeEmpresa: nomeEmpresa.value,
//       email: email.value,
//       cnpj: cnpj.value,
//       // dataLiberacao: dataLiberacao.value,
//       dataCadastro: dataCadastro.value,
//       dataAtualizacao: dataAtualizacao.value,
//       // image: JSON.stringify({
//       //   descricao: descricao.value,
//       //   imageData: imgEmpresa.value,
//       // })
//     }),
//   })
//     .then((res) => res.json())
//     .then((data) => {
//       const postListaEmpresa = [];
//       postListaEmpresa.push(data);
//       console.log(postListaEmpresa);
//     });
// });

function abrirModalConfirmado() {
  return document.getElementById("id02").style.display = "block";
}

function formatarData(campoTexto) {
  campoTexto.value = mascaraDataCad(campoTexto.value);
}

function mascaraDataCad(valor) {
  return valor
  .replace(/\D/g, '')
  .replace(/(\d{2})(\d)/, '$1/$2')
  .replace(/(\d{2})(\d)/, '$1/$2')
  .replace(/(\d{4})(\d)/, '$1')
}

function formatarCampo(campoTexto) {
  campoTexto.value = mascaraCnpj(campoTexto.value);
}

function mascaraCnpj(valor) {
  // return valor.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, "$1.$2.$3/$4-$5");
  return valor
  .replace(/\D/g, '')
  .replace(/(\d{2})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1/$2')
  .replace(/(\d{4})(\d)/, '$1-$2')
  .replace(/(-\d{2})\d+?$/, '$1')
}

function checkForm(empresaForm) {
  event.preventDefault()
  validarForm();
  formatandoDataPost();
  formatandoCNPJ();
  mostrarImgBase64();
}

function validarEmail(email) {
  var regexValid = /^([a-z]){1,}([a-z0-9._-]){1,}([@]){1}([a-z]){2,}([.]){1}([a-z]){2,}([.]?){1}([a-z]?){2,}$/i;
  console.log("É email valido? Resposta: " + regexValid.test(email))
  if(regexValid.test(email)) {
    console.log("Email válido!");
    return true;
  } else {
    console.log("Email inválido!");
    return false
  }
}

function validarForm() {
  this.validarEmail(email.value)
  
  if (nomeEmpresa.value == "") {
    alert("Formulário inválido. Nome da empresa não informado!");
    nomeEmpresa.focus();
    return false;
  }

  if (cnpj.value == "") {
    alert("Formulário inválido. Preencha o campo com um CNPJ válido!");
    cnpj.focus();
    return false;
  }

  if (dataLiberacao.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data cadastro!");
    dataLiberacao.focus();
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
    nomeEmpresa.value != null &&
    email.value != null &&
    cnpj.value != null &&
    dataLiberacao.value != null &&
    imgEmpresa.value != null &&
    descricaoImg.value != null
  ) {
    enviarForms.disabled = true;
    enviarForms.value = "Aguarde um momento...";
    return document.getElementById("id01").style.display = "block";
  }
}

function formatandoDataPost() {
  let dataEnviada = dataLiberacao.value;

  let formatarDataCad = dataEnviada.split('/');
  let dataCadFormatando = formatarDataCad[1] + "/"  + formatarDataCad[0] + "/" + formatarDataCad[2];
  let dataCadFormatada = new Date(dataCadFormatando);
  let dataCadString = JSON.stringify(dataCadFormatada).replace(/"/g, "");
  dataEnviada = dataCadString;
  dataLiberacao.value = dataEnviada;

}

function formatandoCNPJ() {
  let cnpjEnviado = cnpj.value;
  cnpjEnviado = cnpjEnviado.replace(/(\.|\/|\-)/g, "");
  cnpj.value = cnpjEnviado;
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
      console.log("Converted Base64 version is " + document.getElementById("imgTest").innerHTML);
    }
    fileReader.readAsDataURL(fileToLoad);
  }
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaInicia() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    urlSITE + "/Projeto.Web/pages/empresa/lista-empresa.html";
}
