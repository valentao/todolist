using Todolist.Models;
using Microsoft.EntityFrameworkCore;

namespace Todolist.Data
{
    public class TodolistContext : DbContext
    {
        public TodolistContext() { }

        public TodolistContext(DbContextOptions<TodolistContext> options) 
            : base(options) { }


        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<TaskPriority> TaskPriority { get; set; }

        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Todolist;Trusted_Connection=True;");
        //}
    }
}
