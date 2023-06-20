using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodolistMvc.Models.Tasks
{
    public class TaskEditDTO
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Display(Name = "Task priority")]
        public int TaskPriorityId { get; set; }

        [Display(Name = "Deadline")]
        public DateTime? DateDeadline { get; set; }

        [Display(Name = "Parent task")]
        public int? TaskParentId { get; set; }
    }
}
