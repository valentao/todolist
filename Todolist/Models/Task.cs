﻿using System.ComponentModel.DataAnnotations.Schema;
using Todolist.Areas.Identity.Data;

namespace Todolist.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        public TaskPriority TaskPriority { get; set; } = null!;

        public DateTime DateCreate { get; set; }

        public DateTime? DateDone { get; set; }

        public DateTime? DateDeadline { get; set; }

        public bool IsDone { get; set; }

        public Task? TaskParent { get; set; }
    }
}
