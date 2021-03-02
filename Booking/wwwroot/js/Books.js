const uriBooks = window.location.origin + "/api/Books/GetBooks";

let books = [];


// Books
function getBooks() {
    fetch(uriBooks)
        .then(response => response.json())
        .then(data => _displayBooks(data))
        .catch(error => console.error('Unable to get the Books', error));
}

function deleteBook(id) {
    
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this Book file!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willdelete) => {
            if (willdelete) {
                console.log(`${uriBooks}/${id.trim()}`)
                fetch(`${uriBooks}/${id.trim()}`,
                    {
                        method: 'DELETE'
                    })
                    .then(() => getBooks())
                    .catch(error => console.error('Unable to delete the book', error));
                swal("Poof! Your Book file has been deleted!", {
                    icon: "success",
                });
            } else {
                swal("Your Book file is safe!");
            }
        });
}


function _displayCountBooks(equipCount) {
    const name = (equipCount === 1) ? 'Livro cadastrado' : 'Livros cadastrados';
    document.getElementById("counter").innerText = `${equipCount} ${name}`;
}

function _displayAverageValue(values) {
    const name = 'Valor Médio dos Livros: ';
    document.getElementById("valorMdeio").innerText = `${name} ${values}`;
}

function _displayBooks(data) {
    const tBody = document.getElementById("books");
    tBody.innerHTML = '';

    _displayCountBooks(data.length);

    const button = document.createElement('button');
    const a = document.createElement('a');

    let valorLivro = 0;


    data.forEach(book => {

        let editBook = a.cloneNode(false);
        editBook.classList.add('btn');
        editBook.classList.add('btn-secondary');
        editBook.innerText = 'Edit';
        let urlEditBook = window.location.origin + '/book/edit/' + book.id;
        editBook.setAttribute('href', urlEditBook);

        let detailsBook = a.cloneNode(false);
        detailsBook.innerText = 'Details';
        detailsBook.classList.add('btn');
        detailsBook.classList.add('btn-info');
        let urlDetailsBook = window.location.origin + '/books/details/' + book.id;
        detailsBook.setAttribute('href', urlDetailsBook);

        let deleteBook = button.cloneNode(false);
        deleteBook.innerText = 'Delete';
        deleteBook.classList.add('btn');
        deleteBook.classList.add('btn-danger');
        console.log(book.id)
        deleteBook.setAttribute('onclick', 'deleteBook(" ' + book.id + '")');

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(book.code);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(book.title);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(book.price);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        let textNode4 = document.createTextNode(book.dateRegister);
        td4.appendChild(textNode4);

        let td5 = tr.insertCell(4);
        td5.appendChild(editBook);

        //let td6 = tr.insertCell(5);
        //td6.appendChild(detailsBook);

        let td7 = tr.insertCell(5);
        td7.appendChild(deleteBook);
        valorLivro += book.price;
    });
    books = data;
    _displayAverageValue(valorLivro);
}

function SearchBook() {
    var input, filter, tBody, tr, colName, i, textValue;
    var cont = 0;
    input = document.getElementById("searchBook");
    filter = input.value.toUpperCase();
    tBody = document.getElementById("books");
    tr = tBody.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {

        colName = tr[i].getElementsByTagName('td')[1].innerHTML.toUpperCase();


        if (colName.indexOf(filter) > -1) {
            tr[i].style.display = "";
            cont = cont + 1;
            _displayCountBooks(cont)
        } else {
            tr[i].style.display = "none";
            _displayCountBooks(cont)
        }

    }
}

function NewBook() {
    const newBook = document.getElementById("newBook");
    const urlBook = window.location.origin + "/Book/NewBook";
    newBook.setAttribute('href', urlBook);
}

