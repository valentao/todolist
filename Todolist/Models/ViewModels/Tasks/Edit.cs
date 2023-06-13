namespace Todolist.Models.ViewModels.Tasks
{
    public class Edit
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int TaskPriorityId { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateDone { get; set; }

        public DateTime? DateDeadline { get; set; }

        public int? TaskParentId { get; set; }

        public bool IsDone { get; set; }
    }
}
