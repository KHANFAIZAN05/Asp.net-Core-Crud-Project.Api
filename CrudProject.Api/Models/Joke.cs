using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudProject.Api.Models
{
    public class Joke
    {
        public int JokeId { get; set; }
        [Required]
        [Display(Name ="Joke")]
        public string JokeName{ get; set; }
        [Required]
        public decimal Ratings{ get; set; }
    }
}
