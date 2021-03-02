using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Booking.Models;
using Booking.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult NewBook()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            ViewBag.id = id;
            return View(ViewBag);
        }

        [HttpGet]
        public ActionResult CreateBook()
        {
            BookViewModel bookViewModel = new BookViewModel();
            return View(bookViewModel);
        }


        [HttpPost]
        public ActionResult CreateBook(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                List<Book> books = LoadBooks();

                var bookInData = books.SingleOrDefault(b => b.Code == bookViewModel.Code);
                if (bookInData == null && bookViewModel.Code > 0)
                {
                    Book book = new Book(bookViewModel.Code, bookViewModel.Title, bookViewModel.Price, bookViewModel.DateRegister.Date);
                    books.Add(book);
                    if (SaveBook(book))
                        return RedirectToAction("LoadData");
                }
            }

            return View(bookViewModel);
        }

        public bool SaveBook(Book _book)
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
            return result;
        }

        public List<Book> LoadBooks()
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

            return books;
        }

        public ActionResult LoadData()
        {
            List<Book> books = LoadBooks();

            var booksViewModel = new BooksViewModel
            {
                Books = books
            };

            return View(booksViewModel);
        }
    }
}
