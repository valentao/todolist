using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Todolist.Data;
using Todolist.Models;

namespace Todolist.Pages.Tasks
{
    public class DetailsModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;

        public DetailsModel(Todolist.Data.TodolistContext context)
        {
            _context = context;
        }

      public Models.Task Task { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            
            if (task == null)
            {
                return NotFound();
            }
            else 
            {
                var taskParent = await _context.Tasks.FirstOrDefaultAsync(p => p.Id == task.TaskParentId);
                
                Task = task;
                if (taskParent != null)
                {
                    Task.TaskParent = taskParent;
                }
            }
            return Page();
        }
    }
}
