using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Todolist.Areas.Identity.Data;
using Todolist.Data;
using Todolist.Models;

namespace Todolist.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;

        private List<TaskPriority> taskPriorities = new List<TaskPriority>();

        public IndexModel(Todolist.Data.TodolistContext context)
        {
            _context = context;
        }

        public IList<Models.Task> Task { get; set; } = default!;

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            taskPriorities = await _context.TaskPriority.ToListAsync();
            //users = await _contextAuth.Users.ToListAsync();

            if (_context.Tasks != null)
            {
                Task = await _context.Tasks.ToListAsync();
                foreach (Models.Task task in Task)
                {
                    //task.TaskPriority = (TaskPriority)taskPriorities.Where(x => x.Id == task.TaskPriorityId);
                    task.TaskPriority = taskPriorities.FirstOrDefault(taskPriority => taskPriority.Id == task.TaskPriorityId);
                    task.TaskParent = Task.FirstOrDefault(taskParent => taskParent.Id == task.TaskParentId);
                    //task.User = users.FirstOrDefault(u => u.Id == task.UserId);
                }
            }
        }

        /// <summary>
        /// Remove all tasks
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostClear()
        {
            // remove task and all his subordinate
            var removeTasks = await _context.Tasks.ToListAsync();
            if (removeTasks.Count() > 0)
            {
                _context.Tasks.RemoveRange(removeTasks);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        /// <summary>
        /// Mark selected task and his subordinates like done
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDone(int id)
        {
            _context.Tasks.Where(t => t.Id == id || t.TaskParentId == id).ToList().ForEach(x =>
            {
                x.IsDone = true;
                x.DateDone = DateTime.Now;
            });

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
