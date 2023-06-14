using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using TodolistMvc.Areas.Identity.Data;
using TodolistMvc.Models;

public class TaskContext : DbContext
{
    public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
    {
    }

    public DbSet<TodolistMvc.Models.Task> Task { get; set; } = default!;

    public DbSet<TodolistMvc.Models.TaskPriority> TaskPriority { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //builder.Entity<TodolistMvc.Models.TaskNew>().ToTable("Task");
        //builder.Entity<TodolistMvc.Models.TaskEdit>().ToTable("Task");

        builder.Entity<ApplicationUser>().Metadata.SetIsTableExcludedFromMigrations(true);

        builder.Entity<TaskPriority>().HasData(
                new TaskPriority
                {
                    Id = 1,
                    Name = "High",
                    Description = "Highest level of priority",
                    Color = "#ff0000"
                },
                new TaskPriority
                {
                    Id = 2,
                    Name = "Medium",
                    Description = "Medium level of priority",
                    Color = "#ffa500"
                },
                new TaskPriority
                {
                    Id = 3,
                    Name = "Low",
                    Description = "Lowest level of priority",
                    Color = "#008000"
                }
                );
    }
}
