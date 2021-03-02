const bookId = document.getElementById("bookId").value;
console.log(typeof(bookId))

const uriBook = window.location.origin + "/api/Books/GetBookById/" + bookId;

const updateButton = document.getElementById('update');
const cancelButton = document.getElementById('cancelar');

function getBook() {
    fetch(uriBook)
        .then(response => response.json())
        .then(data => {
            console.log(typeof(data.dateRegister))
            document.getElementById('add-code').value = data.code;
            document.getElementById('add-title').value = data.title;
            document.getElementById('add-price').value = data.price;
            let tmp = data.dateRegister;
            let tmp1 = tmp.split('T')[0];
            document.getElementById('add-date').value = tmp1;
        })
        .catch(error => console.error('Unable to get the Books', error));
}

getBook();



updateButton.addEventListener("click", function updateBook() {

    console.log(1);

    const book = {
        id: bookId,
        code: parseInt(document.getElementById('add-code').value),
        title: document.getElementById('add-title').value,
        price: parseFloat(document.getElementById('add-price').value),
        dateRegister: document.getElementById('add-date').value
    }

    const uriPutBook = window.location.origin + "/api/Books/" + `${bookId}`;
    console.log(uriPutBook);

    swal({
        title: "Livro editado!",
        text: "Seu livro foi editado com sucesso!",
        icon: "success",
        button: "Ok!",
    }).then(() => {
        fetch(uriPutBook, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-type': 'application/json'
            },
            body: JSON.stringify(book)
        }).then(() => {
            console.log(3);
            const urlList = window.location.origin + "/book/list";
            location.replace(urlList);
        }).catch(error => console.error('Unable to update book.', error));
    });

    
})
    
    

cancelButton.addEventListener('click', function () {
    const urlList = window.location.origin + "/book/list";
    location.replace(urlList);
})        