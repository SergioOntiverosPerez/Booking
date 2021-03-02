using Booking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.ViewModels
{
    public class BookViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Codigo do Livro")]
        public int Code { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "Titulo do Livro")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Preço do Livro")]
        public float Price { get; set; }
        [Required]
        [Display(Name = "Data de cadastro")]
        public DateTime DateRegister { get; set; }

        public BookViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public BookViewModel(Book book)
        {
            Id = book.Id;
            Code = book.Code;
            Title = book.Title;
            Price = book.Price;
            DateRegister = book.DateRegister;
        }
    }
}
