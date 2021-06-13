const listaEmpresa = document.querySelector(".app");
const editarEmpresa = document.querySelector(".editar-departamento");

let dadosEmpresa = "";

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

var variavel = queryString("idEmpresa");
const urlAPIidEmpresa = urlAPI + "/empresa/" + variavel;
console.log(variavel);

const exibirEmpresa = (u) => {
  dadosEmpresa += `    
  <form method="PUT" name="formEmpresa" onsubmit="return checkForm(this)>

  <br />
  <div>
  <label for="nomeEmpresa">Nome da Empresa:</label>
  <input type="text" id="nomeEmpresa" name="nomeEmpresa" value=${u.nomeEmpresa} />
  </div>
  <br />
  
  <br />
  <div>
  <label for="email">E-mail:</label>
  <input type="email" id="email" name="email" value=${u.email} /> <span id="error-email"></span>
  </div>
  <br />
  
  <div>
  <label for="cnpj">CNPJ</label>
  <input
    type="text"
    id="cnpj"
    maxlength="18"
    name="cnpj"
    onkeyup="formatarCampo(this)"
    value=${u.cnpj}
  />
  </div>
  <br />
  
  <div>
  <label for="dataLiberacao">Data Liberação </label>
  <input type="input" id="dataLiberacao" name="dataLiberacao" onkeyup="formatarData(this)" value=${u.dataLiberacao} />
  </div>
  <br />          

  <div>

  <label for="imgEmpresa">Imagem da empresa </label>
  <input type="file" id="imgEmpresa" name="imgEmpresa" onload="mostrarImgBase64()" value="${u.image.imageData}" />

  </div>

  <div id="imgTest" value=${u.image.imageData} onload="mostrarImgBase64()" ></div>

  <div>
    <label for="nomeImgEmpresa">Descricao da Imagem </label>
    <input type="input" id="nomeImgEmpresa" name="nomeImgEmpresa" value="${u.image.descricao}" />
  </div>
  <br />

        <button
        type="submit"
        id="enviarFormEmp"
        class="buttonload"
        onclick= "putEmpresa()"
      >
        Editar Empresa
      </button>
  <form>

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
          <p>Sua empresa foi atualizado com sucesso!</p>

          <div class="clearfix">
            <button type="button" class="confirmbtn" id="confirmar-cadastro" class="confirmbtn" onclick="redirecionarPaginaLista()">Continuar</button>
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
  listaEmpresa.innerHTML = dadosEmpresa;

  let element = document.getElementById('cnpj');
  element.dispatchEvent(new KeyboardEvent('keyup',{'key':'a'}));

  let dataL = document.getElementById('dataLiberacao');
  dataL.dispatchEvent(new KeyboardEvent('keyup',{'key':'b'}));

  // var image = new Image();
  // image.onload = function () {
  //   ctx.drawImage(image, 0, 0);
  // };
  // image.src = imgEmpresa.value;

  // console.log(imgEmpresa.value);

  var image = new Image();
  console.log(u.image.imageData);

  image.src = u.image.imageData;
  document.getElementById('imgTest').appendChild(image);

  // document.getElementById('imgTest').innerHTML = image;
};

fetch(urlAPIidEmpresa)
  .then((s) => s.json())
  .then((dados) => exibirEmpresa(dados));

// METODO PUT

// var data = window.prompt("Digite a sua string:");

// if (data) {
//     var b64 = window.btoa(data);
//     var b64dois = window.atob(b64)
//     console.log("Codificado: " + b64)
//     console.log("Decodificado: " + b64dois);
// } else {
//     alert("Nada foi digitado");
// }

async function putEmpresa() {
  event.preventDefault();
  this.checkForm();
  
  let dados;
  let elemento = document.getElementById("imgEmpresa");

  if (!elemento.value) {
    let pegandoValor =  document.getElementById("imgTest").innerHTML;
    let formatandoValor = pegandoValor.split('"');
    dados = formatandoValor[1];
  }
  else {
    dados = elemento.files[0];
    let reader = new FileReader();
    dados = new Promise((resolve) => {
      reader.addEventListener("load", () => {
        resolve(reader.result);
      });
      reader.readAsDataURL(dados);
    });
    dados = await dados;
  }

  try {
    const atualizarEmpresa = await fetch(urlAPIidEmpresa, {
      method: "PUT",
      body: JSON.stringify({
        nomeEmpresa: nomeEmpresa.value,
        email: email.value,
        cnpj: cnpj.value,
        dataLiberacao: dataLiberacao.value,
        image: {
          descricao: nomeImgEmpresa.value,
          imageData: dados,
        },
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

function formatarData(campoTexto) {
  campoTexto.value = mascaraDataCad(campoTexto.value);
}

function mascaraDataCad(valor) {
  let dataEnviada = valor;

  let formatandoDataCad = dataEnviada.split("T")
  let formatarDataCad = formatandoDataCad[0]
  let formatacaoDataCad = formatarDataCad.split("-")
  let dataFormatada = formatacaoDataCad[2] +  formatacaoDataCad[1] + formatacaoDataCad[0]
  dataEnviada = dataFormatada;
  valor = dataEnviada;
  
  return valor
    .replace(/\D/g, "")
    .replace(/(\d{2})(\d)/, "$1/$2")
    .replace(/(\d{2})(\d)/, "$1/$2")
    .replace(/(\d{4})(\d)/, "$1");
}

function formatarCampo(campoTexto) {
  campoTexto.value = mascaraCnpj(campoTexto.value);
}

function mascaraCnpj(valor) {
  return valor
    .replace(/\D/g, "")
    .replace(/(\d{2})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1.$2")
    .replace(/(\d{3})(\d)/, "$1/$2")
    .replace(/(\d{4})(\d)/, "$1-$2")
    .replace(/(-\d{2})\d+?$/, "$1");
}

function checkForm(empresaForm) {
  event.preventDefault();
  validarForm();
  formatandoDataPost();
  formatandoCNPJ();
  mostrarImgBase64();
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
  const enviarForms = document.getElementById("enviarFormEmp");
  const nomeEmpresa = document.getElementById("nomeEmpresa");
  const email = document.getElementById("email");
  const cnpj = document.getElementById("cnpj");
  const dataLiberacao = document.getElementById("dataLiberacao");
  const imgEmpresa = document.getElementById("imgEmpresa");
  const nomeImgEmpresa = document.getElementById("nomeImgEmpresa");

  this.validarEmail(email.value);

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

  if (nomeImgEmpresa.value == "") {
    alert("Formulário inválido. Preencha o campo com a Data atualizacão!");
    nomeImgEmpresa.focus();
    return false;
  }

  if (
    nomeEmpresa.value != null &&
    email.value != null &&
    cnpj.value != null &&
    dataLiberacao.value != null &&
    nomeImgEmpresa.value != null
  ) {
    enviarForms.disabled = true;
    enviarForms.value = "Aguarde um momento...";
    return (document.getElementById("id01").style.display = "block");
  }
}

function formatandoDataPost() {
  let dataEnviada = dataLiberacao.value;

  let formatarDataCad = dataEnviada.split("/");
  let dataCadFormatando =
    formatarDataCad[1] + "/" + formatarDataCad[0] + "/" + formatarDataCad[2];
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
  // var imagemSelecionado = imgEmpresa.value;
  // if (imagemSelecionado.length > 0) {
  //   var fileReader = new FileReader();
  //   fileReader.onload = function (event) {
  //     var urlImg = event.target.result; // <--- data: base64
  //     var newImage = document.createElement("img");
  //     newImage.style = "width: 300px; margin: 10px";
  //     newImage.src = urlImg;
  //     document.getElementById("imgTest").innerHTML = newImage.outerHTML;
  //     console.log(
  //       "Converted Base64 version is " +
  //         document.getElementById("imgTest").innerHTML
  //     );
  //   };
  //     fileReader.readAsDataURL(imagemSelecionado);
  //   }
}

function redirecionarPaginaInicial() {
  location.href = urlSITE + "/index.html";
}

function redirecionarPaginaLista() {
  location.href =
    urlSITE + "/Projeto.Web/pages/empresa/lista-empresa.html";
}
