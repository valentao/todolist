namespace Todolist.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int UserId { get; set; }

        public int TaskPriorityId { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateDone { get; set; }

        public DateTime? DateDeadline { get; set; }

        public bool IsDone { get; set; }

        public int? TaskId { get; set; }
    }
}
