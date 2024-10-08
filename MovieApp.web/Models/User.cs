﻿using movieApp.web.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    }
}
