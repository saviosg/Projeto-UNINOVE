const fTeste = document.getElementById('formTeste');

formTeste.addEventListener('submit', (e) => {
    e.preventDefault();

    const idEmpresa = document.getElementById('id_empresa').value;
    const idUsuario = document.getElementById('id_usuario').value;

    heads = new Headers();

    // POST

    fetch('https://localhost:44379/teste', {
        method: 'POST',
        body: JSON.stringify({
            id_empresa: idEmpresa,
            id_usuario: idUsuario,
        }),
        cache: 'no-cache',
        credentials: 'same-origin', 
        headers: heads,
        mode: 'cors',
    }).then(response => {
        console.log('Response', response);
        return response.text();
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });

    // GET
    fetch('https://localhost:44379/teste', {
        method: 'GET'
    })
    .then(response => {
        return response.json
    }).then(data => {
        console.log(data);
    })

});


// GET ID

// PUT

// DELET