using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoAlbum.Models
{
    public class Photo
    {
        // Primary key
        [Display(Name="Id")]
        public int PhotoId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Filename { get; set; } = string.Empty;

        public string Camera {  get; set; } = string.Empty;

        [Display(Name = "Created")]
        public DateTime CreateDate { get; set; }

        // Foreign key
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Navigation Property
        public Category? Category { get; set; } // nullable

        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? FormFile { get; set; } // nullable


    }

}
