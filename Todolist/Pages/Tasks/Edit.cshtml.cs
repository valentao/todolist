using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todolist.Data;
using Todolist.Models;
using Todolist.Models.ViewModels.Tasks;

namespace Todolist.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;

        /// <summary>
        /// Select list for TaskPriority dropdown
        /// </summary>
        public SelectList TaskPrioritySL { get; set; } = null!;

        /// <summary>
        /// Select list for TaskParent drodown
        /// </summary>
        public SelectList TaskSL { get; set; } = null!;

        public EditModel(Todolist.Data.TodolistContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Read data for TaskPriority list
        /// AsNoTracking - data (table TaskPriority) is not expected to change while the application is running
        /// </summary>
        /// <param name="selectedTaskPriority"></param>
        public void TaskPriorityDropDownList(object? selectedTaskPriority = null)
        {
            TaskPrioritySL = new SelectList(_context.TaskPriority.AsNoTracking(),
                nameof(TaskPriority.Id),
                nameof(TaskPriority.Name),
                selectedTaskPriority);
        }

        /// <summary>
        /// Read data for TaskParent list
        /// Select tasks with no TaskParentId - only one level of nesting
        /// </summary>
        /// <param name="selectedTask"></param>
        public void TaskDropDownList(object? selectedTask = null)
        {
            TaskSL = new SelectList(_context.Tasks.Where(t => t.TaskParentId == null),
                nameof(TaskPriority.Id),
                nameof(TaskPriority.Name),
                selectedTask);
        }

        [BindProperty]
        public Edit TaskEdit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            TaskPriorityDropDownList();
            TaskDropDownList();

            var task = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            Edit taskDetail = new Edit()
            {
                Id = task.Id,
                Name = task.Name,
                DateCreate = task.DateCreate,
                DateDone = task.DateDone,
                DateDeadline = task.DateDeadline,
                TaskPriorityId = task.TaskPriorityId,
                TaskParentId = task.TaskParentId,
                IsDone = task.IsDone
            };

            TaskEdit = taskDetail;

            //Task = task;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TaskPriorityDropDownList();
                TaskDropDownList();
                return Page();
            }

            // Convert ViewModel TaskCreate to entity model Task
            Models.Task task = new Models.Task
            {
                Id = TaskEdit.Id,
                Name = TaskEdit.Name,
                DateCreate = TaskEdit.DateCreate,
                DateDone = TaskEdit.DateDone,
                TaskPriorityId = TaskEdit.TaskPriorityId,
                DateDeadline = TaskEdit.DateDeadline,
                TaskParentId = TaskEdit.TaskParentId,
                IsDone = TaskEdit.IsDone
            };

            _context.Attach(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
