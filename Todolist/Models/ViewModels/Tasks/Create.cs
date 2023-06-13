namespace Todolist.Models.ViewModels.Tasks
{
    public class Create
    {
        public string Name { get; set; } = null!;

        public int TaskPriorityId { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateDeadline { get; set; }

        public int? TaskParentId { get; set; }
    }
}
