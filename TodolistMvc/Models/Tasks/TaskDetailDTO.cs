﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TodolistMvc.Models.Tasks
{
    public class TaskDetailDTO
    {

        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Display(Name = "Task priority")]
        public int TaskPriorityId { get; set; }

        [Display(Name = "Task priority")]
        public TaskPriority TaskPriority { get; set; } = null!;

        [Display(Name = "Date Create")]
        public DateTime DateCreate { get; set; }

        [Display(Name = "Deadline")]
        public DateTime? DateDeadline { get; set; }

        [Display(Name = "Date done")]
        public DateTime? DateDone { get; set; }

        [Display(Name = "Is Done")]
        public bool IsDone { get; set; }

        [Display(Name = "Parent task")]
        public int? TaskParentId { get; set; }

        [Display(Name = "Parent task")]
        public Task? TaskParent { get; set; }
    }
}
