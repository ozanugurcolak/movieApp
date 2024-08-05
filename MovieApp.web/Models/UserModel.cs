using movieApp.web.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(10,ErrorMessage ="Maximum 10 karakter olmalı.")]
        public string UserName { get; set; }
        [Required]
        [StringLength(15,ErrorMessage ="{0} karakter uzunluğu {2}-{1} arasında olmalıdır.",MinimumLength =3)]
        public string Name { get; set; }
        [EmailProviders]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
        //[Range(1900,2014)]
        //public int BirthYear { get; set; }

        [BirthDate(ErrorMessage ="Doğum tarihiniz belirttiğiniz tarih olamaz.")]
        [DataType(DataType.Date)]
        [Display(Name ="Birth Date")]
        public DateTime BirthDate { get; set; }


    }
}
