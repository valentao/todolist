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
    public class DeleteModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;

        public DeleteModel(Todolist.Data.TodolistContext context)
        {
            _context = context;
        }

        [BindProperty]
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
                Task = task;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }
            var task = await _context.Tasks.FindAsync(id);

            // edit subordinated tasks - remove link to parent - one by one
            //var subordinateTasks = _context.Tasks.Where(t => t.TaskParentId == id);

            //if (subordinateTasks.Count() > 0)
            //{
            //    foreach (Models.Task sTask in subordinateTasks)
            //    {
            //        sTask.TaskParentId = null;
            //        _context.Tasks.Update(sTask);
            //    }
            //}

            // edit subordinate tasks - remove link to parent - one Linq query
            _context.Tasks.Where(t => t.TaskParentId == id).ToList().ForEach(x =>
            {
                x.TaskParentId = null;
            });

            // remove subordinated tasks
            //if (subordinateTasks.Count() > 0)
            //{
            //    _context.Tasks.RemoveRange(subordinateTasks);
            //}

            // remove task and all his subordinate
            //var removeTasks = _context.Tasks.Where(t => t.Id == id || t.TaskParentId == id);
            //if (removeTasks.Count() > 0)
            //{
            //    _context.Tasks.RemoveRange(removeTasks);
            //}

            if (task != null)
            {
                Task = task;
                _context.Tasks.Remove(Task);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
