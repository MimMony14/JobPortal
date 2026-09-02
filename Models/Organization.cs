using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }

        [Required]
        public string OrganizationName { get; set; } = string.Empty;

        public string? Website { get; set; }   // nullable

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public ICollection<Job>? Jobs { get; set; } // nullable
    }
}
