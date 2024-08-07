using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using movieApp.web.Entity;

namespace movieApp.web.Models
{
    public class MoviesViewModel
    {
        public List<Movie> Movies { get; set; }
    }
}