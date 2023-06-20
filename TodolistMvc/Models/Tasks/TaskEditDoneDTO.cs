using System.ComponentModel.DataAnnotations;

namespace TodolistMvc.Models.Tasks
{
    public class TaskEditDoneDTO
    {
        public int Id { get; set; }

        [Display(Name = "Date done")]
        public DateTime? DateDone { get; set; }

        [Display(Name = "Is Done")]
        public bool IsDone { get; set; }
    }
}
