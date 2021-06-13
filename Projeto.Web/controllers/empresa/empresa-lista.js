const listaEmpresa = document.querySelector(".lista-empresa");
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
let dadosEmpresa = "";

const exibirDepartamentos = (u) => {
  Array.from(u).forEach((empresa) => {
    dadosEmpresa += `
        <tr>
        <td class="nomeEmp">${empresa.idEmpresa}</td>
        <td class="nomeEmp">${empresa.nomeEmpresa}</td>
        <td class="emailCad">${empresa.email}</td>
        <td class="cnpjCad" id="cnpjList">${mascaraCnpj(empresa.cnpj)}</td>
        <td class="dataCadastroCad">${mascaraData(empresa.dataCadastro)}</td>
        <td class="dataAtualizacaoCad">${mascaraData(empresa.dataAtualizacao)}</td>
        <td class="dataAtualizacaoCad">${mascaraData(empresa.dataLiberacao)}</td>
        <td>
          <button id="atualiza-empresa" onclick="editarItem(${empresa.idEmpresa})">Editar</button>
        </td>
        <td>
          <button class="deletebtn" onclick="removeItem(${empresa.idEmpresa})">Excluir</button>
        </td>
    </tr>
        `;
  });
  listaEmpresa.innerHTML = dadosEmpresa;
};

// METODO GET

fetch(urlAPI + "/empresa")
  .then((s) => s.json())
  .then((dados) => exibirDepartamentos(dados));

// METODO POST

async function putEmpresa() {
  console.log("teste");
  const atualizarEmpresa = await fetch(
    urlAPI + "/empresa/" + idEmpresa.value,
    {
      method: "PUT",
      body: JSON.stringify({
        nomeEmpresa: nomeEmpresa.value,
        email: email.value,
        cnpj: cnpj.value,
        // dataLiberacao: dataLiberacao.value,
        dataCadastro: dataCadastro.value,
        dataAtualizacao: dataAtualizacao.value,
        // image: {
        //   descricao: descricao.value,
        //   imageData: imgEmpresa.value,
        // }
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    }
  )
    .then((response) => response.json())
    .then((json) => console.log(json));
  console.log(atualizarEmpresa);
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

function mascaraData(data) {

  let formatarData = data.split("-");
  let dataFormatando = formatarData[2] + "/"  + formatarData[1] + "/" + formatarData[0];
  let dataFormatandos = dataFormatando.split("T00:00:00");
  let dataFormatada = dataFormatandos[0] + dataFormatandos[1];

  data = dataFormatada;
  return data;
}


function editarItem(empresaId) {
  console.log("Pegando ID da empresa", empresaId);
  
  localStorage.setItem('idEmpresa', empresaId);

  window.location = "editar-empresa?idEmpresa=" + empresaId;

  // window.href =
  // urlSITE + "/Projeto.Web/pages/empresa/editar-empresa.html";
}

// DELETE
async function removeItem(empresaId) {
  var url = urlAPI + "/empresa/" + empresaId;
  const empresa = await fetch(url, {
    method: "DELETE",
  })
    .then((response) => {
      console.log(response.json());
      location.reload();
    });
}

// function formatarImgBase64() {
//   var arquivoSelecionado = document.getElementById("imgEmpresa").files;
//   if (arquivoSelecionado.length > 0) {
//     var fileToLoad = arquivoSelecionado[0];

//     var fileReader = new FileReader();

//     fileReader.onload = function (fileLoadedEvent) {
//       var srcData = fileLoadedEvent.target.result; // <--- data: base64

//       var newImage = document.createElement("img");
//       newImage.src = srcData;

//       document.getElementById("imgTest").innerHTML = newImage.outerHTML;
//       alert(
//         "Converted Base64 version is " +
//           document.getElementById("imgTest").innerHTML
//       );
//       console.log(
//         "Converted Base64 version is " +
//           document.getElementById("imgTest").innerHTML
//       );
//     };
//     fileReader.readAsDataURL(fileToLoad);
//   }
// }

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

function abrirModalConfirmado() {
  return (document.getElementById("id02").style.display = "block");
}

