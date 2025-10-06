using System.ComponentModel.DataAnnotations;

namespace JobAPI.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
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

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
