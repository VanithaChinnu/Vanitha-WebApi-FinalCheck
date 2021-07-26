using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary
{
    [Table("Movie")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Title { get; set; }
        public string BoxOffice { get; set; }
        public bool Active { get; set; }
        public string Genre { get; set; }
        public DateTime DateOfLaunch { get; set; }
        public bool HasTeaser { get; set; }
        public bool Favorite { get; set; }
    }
}