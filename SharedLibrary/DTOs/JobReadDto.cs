namespace SharedLibrary.DTOs
{
    public class JobReadDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string? Location { get; set; }

        public decimal Salary { get; set; }

        public int Experience { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}