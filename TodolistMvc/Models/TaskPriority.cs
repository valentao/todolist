namespace TodolistMvc.Models
{
    public class TaskPriority
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Color { get; set; } = null!;

    }
}
