namespace PhotoAlbum.Models
{
    public class Category
    {
        // Primary key
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        // Navigation Property
        public List<Photo>? Photos { get; set; } // nullable
    }
}
