using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Models
{
    public class Book : BookingBase
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

        public Book()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Book(int code) : this()
        {
            Code = code;
        }

        public Book(int code, string title) : this(code)
        {
            Title = title;
        }

        public Book(int code, string title, float price) : this(code, title)
        {
            Price = price;
        }

        public Book(int code, string title, float price, DateTime date) : this(code, title, price)
        {
            DateRegister = date;
        }

        public Book(string id, int code, string title, float price, DateTime date)
        {
            Id = id;
            Code = code;
            Title = title;
            Price = price;
            DateRegister = date;
        }
    }
}
