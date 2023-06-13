using Todolist.Models;
using Microsoft.EntityFrameworkCore;
using Todolist.Areas.Identity.Data;

namespace Todolist.Data
{
    public class TodolistContext : DbContext
    {
        public TodolistContext() { }

        public TodolistContext(DbContextOptions<TodolistContext> options)
            : base(options) { }


        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<TaskPriority> TaskPriority { get; set; }

        public DbSet<ApplicationUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskPriority>().HasData(
                new TaskPriority
                {
                    Id = 1,
                    Name = "High",
                    Description = "Highest level of priority",
                    Color = "#ff0000"
                },
                new TaskPriority
                {
                    Id= 2,
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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Todolist;Trusted_Connection=True;");
        //}
    }
}
