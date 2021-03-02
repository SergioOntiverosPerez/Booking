using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Booking.Models;
using Booking.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers.Api
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        // GET: api/Books
        [HttpGet]
        [Route("GetBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await LoadBooks();
        }

        // GET: api/books/id
        [HttpGet("{id}")]
        [Route("GetBookById/{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            List<Book> books = await LoadBooks();
            var book = books.SingleOrDefault(e => e.Id == id);
            if (book == null)
                return NotFound();

            return book;
        }

        [HttpGet("{code}")]
        [Route("GetBookByCode/{code}")]
        public async Task<ActionResult<Book>> GetBookByCode(int code)
        {
            List<Book> books = await LoadBooks();
            var book = books.Single(b => b.Code == code);
            if (book == null)
                return NotFound();

            return book;
        }


        // POST /api/books
        [HttpPost]
        [Route("PostBook")]
        public async Task<ActionResult<Book>> PostBook(Book _book)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            List<Book> books = await LoadBooks();
            var bookInData = books.SingleOrDefault(b => b.Code == _book.Code);
            if(bookInData == null && _book.Code > 0)
            {
                Book book = new Book(_book.Code, _book.Title, _book.Price, _book.DateRegister);

                books.Add(book);

                if (SaveBook(book).Result)
                    return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
                else
                    return NoContent();
            }

            return _book;
        }


        // PUT api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, Book _book)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            List<Book> books = await LoadBooks();
            var bookInDb = books.FirstOrDefault(p => p.Id == id);
            
            if (bookInDb == null)
                return NotFound();

            var bookInDbCOde = books.FirstOrDefault(b => b.Code == _book.Code);

            if(bookInDbCOde == null || bookInDbCOde.Code != _book.Code && _book.Code > 0)
            {
                bookInDb.Title = _book.Title;
                bookInDb.Code = _book.Code;
                bookInDb.Price = _book.Price;
                bookInDb.DateRegister = _book.DateRegister;

                if (!UpdateDataBase(bookInDb).Result)
                    throw new Exception("It was impossible to delete");

                return NoContent();
            }
            return BadRequest();
        }


        // Delete /api/books/1
        [HttpDelete("{id}")]
        public void DeleteBook(string Id)
        {
            var books =  LoadBooks();

            var bookInDb = (books.Result).SingleOrDefault(c => c.Id == Id);

            if (bookInDb == null)
                throw new ArgumentException("Book not found");

            if (!DeleteContactInDB(bookInDb.Id).Result)
                throw new Exception("It was impossible to delete");
        }

        private async Task<bool> DeleteContactInDB(string id)
        {
            bool deleted = false;
            List<Book> books = await LoadBooks();

            var book = books.FirstOrDefault(c => c.Id == id);

            if (book != null)
            {
                books.Remove(book);
                deleted = await PrintData(books);
            }

            return deleted;
        }

        private async Task<bool> UpdateDataBase(Book book)
        {
            List<Book> books = await LoadBooks();

            var indexOf = books.IndexOf(books.Find(c => c.Id == book.Id));
            books[indexOf] = book;

            return await PrintData(books);
        }
        private async Task<bool> PrintData(List<Book> _books)
        {
            bool printed = false;
            string path = @"D:\Usuario\Documents\AgafarmaTest\data.txt";
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                foreach (var _book in _books)
                {
                    sw.WriteLine(_book.Id + "#" + _book.Code + "#" + _book.Title + "#" + _book.Price + "#" + _book.DateRegister.Date);
                }
                printed = true;
                sw.Close();
            }
            return await Task.FromResult(printed);
        }

        private async Task<bool> SaveBook(Book _book)
        {
            var result = false;
            string path = @"D:\Usuario\Documents\AgafarmaTest\data.txt";
            try
            {
                using (StreamWriter sappend = System.IO.File.AppendText(path))
                {
                    sappend.WriteLine(_book.Id + "#" + _book.Code + "#" + _book.Title + "#" + _book.Price + "#" + _book.DateRegister.Date);
                    sappend.Close();
                }
                result = true;
            }
            catch (Exception)
            {

            }
            return await Task.FromResult(result);
        }

        private async Task<List<Book>> LoadBooks()
        {
            string path = @"D:\Usuario\Documents\AgafarmaTest\data.txt";
            string[] lines = System.IO.File.ReadAllLines(path);

            var books = new List<Book>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split('#');
                var id = line[0];
                var code = Convert.ToInt32(line[1]);
                var title = line[2];
                var price = (float)Convert.ToDecimal(line[3]);
                var dateregister = Convert.ToDateTime(line[4]);

                Book book = new Book(id, code, title, price, dateregister);
                books.Add(book);
            }

            return await Task.FromResult(books);
        }
    }
}
