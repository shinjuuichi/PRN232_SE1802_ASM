using SharedLibrary.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs
{
    public class JobUpdateDto
    {
        [Required]
        [MaxLength(255)]
        [NotContainsOnlineOrPartTime]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Company { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Location { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public int Experience { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }
    }
}