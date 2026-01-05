using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace JobPortal.Models
{
    public class Apply
    {
        public int Id { get; set; }
        public int JobId { get; set; } 
        public virtual Job? Job { get; set; }    // navigation property

        // Applicant info
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        // Resume
        public byte[] ResumeData { get; set; } = Array.Empty<byte>();
        public string ResumeFileName { get; set; } = "";
        public string ResumePath { get; set; } = "";

        // Cover Letter
        public byte[] CoverLetterData { get; set; } = Array.Empty<byte>();
        public string CoverLetterFileName { get; set; } = "";
        public string CoverLetterPath { get; set; } = "";

        // Application status
        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        // Upload only
        [NotMapped] public IFormFile? ResumeFile { get; set; }
        [NotMapped] public IFormFile? CoverLetterFile { get; set; }
    }
}
