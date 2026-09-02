using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models
{
    public class Bookmark
    {
        [Key]
        public int BookmarkId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int JobId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
    }
}
