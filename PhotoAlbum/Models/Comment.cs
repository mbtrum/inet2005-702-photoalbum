namespace PhotoAlbum.Models
{
    public class Comment
    {
        // Primary key
        public int CommentId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }

        // Foreign key
        public int PhotoId { get; set; }

        // Navigation Property
        public Photo? Photo { get; set; } // nullable
    }
}
