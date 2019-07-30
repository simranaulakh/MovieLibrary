using System;
using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Models
{

    public class Movie 
    {
        [Key]
        public string MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieLanguage { get; set; }
        public string MovieCategory { get; set; }
    }
}
