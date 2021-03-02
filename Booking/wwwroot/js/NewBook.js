const uriBooks = window.location.origin + "/api/Books/PostBook";
console.log(uriBooks)
// Books

const saveButton = document.getElementById("save");
const cancelButton = document.getElementById('cancelar');


const addCodeTextBox = document.getElementById('add-code');
const addTitleTextBox = document.getElementById('add-title');
addTitleTextBox.disabled = true;
const addPriceTextBox = document.getElementById('add-price');
addPriceTextBox.disabled = true
const addDate = document.getElementById('add-date');
addDate.disabled = true;

function searchExistingCode(event){
    const code = document.getElementById("add-code").value;
    parseInt(code);
    const urlGetBookByCode = window.location.origin + "/api/Books/GetBookByCode/" + `${parseInt(code)}`
    fetch(urlGetBookByCode)
        .then(response => response.json())
        .then(book => {
                addTitleTextBox.disabled = false;
                addTitleTextBox.value = book.title;
                addPriceTextBox.disabled = false;
                addPriceTextBox.value = book.price;
                addDate.disabled = false;
                
                let tmp = book.dateRegister;
                let tmp1 = tmp.split('T')[0];
                addDate.value = tmp1;  
        }).catch(error => {
            console.error('Unable to get the book', error)
            addTitleTextBox.disabled = false;
            addPriceTextBox.disabled = false;
            addDate.disabled = false;
            addTitleTextBox.value = '';
            addPriceTextBox.value = '';
            addDate.value = '';
        })

}

document.getElementById("add-code").addEventListener("change", searchExistingCode)

function getValuesOfBook() {
    
    const book = {
        code: parseInt(addCodeTextBox.value),
        title: addTitleTextBox.value,
        price: parseFloat(addPriceTextBox.value),
        dateRegister: addDate.value
    };

    console.log(book);
    console.log(JSON.stringify(book));

    return book;
}



saveButton.addEventListener("click", function addBook() {

    console.log(1)

    book = getValuesOfBook();

    swal({
        title: "Livro salvo!",
        text: "Seu livro foi salvo com sucesso!",
        icon: "success",
        button: "Ok!",
    }).then(() => {
        fetch(uriBooks, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(book)
        }).then(() => {
            addCodeTextBox.value = '';
            addTitleTextBox.value = '';
            addPriceTextBox.value = '';
            addDate.value = '';
        }).then(response => {
            const urlList = window.location.origin + "/book/list";
            location.replace(urlList);
        }).catch(error => console.error('Unable to add book.', error));

    });
})


cancelButton.addEventListener('click', function () {
    const urlList = window.location.origin + "/book/list";
    location.replace(urlList);
}) 