using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodolistMvc.Models
{
    public class TaskPriority
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "nvarchar(255)")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Color { get; set; } = null!;

    }
}
