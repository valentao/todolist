using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Todolist.Areas.Identity.Data;
using Todolist.Data;
using Todolist.Models;
using Todolist.Models.ViewModels.Tasks;

namespace Todolist.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Select list for TaskPriority dropdown
        /// </summary>
        public SelectList TaskPrioritySL { get; set; } = null!;

        /// <summary>
        /// Select list for TaskParent drodown
        /// </summary>
        public SelectList TaskSL { get; set; } = null!;

        /// <summary>
        /// String for saving login user id
        /// </summary>
        public string UserId { get; set; } = null!;

        public CreateModel(Todolist.Data.TodolistContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public IActionResult OnGet()
        {
            TaskPriorityDropDownList(_context);
            TaskDropDownList(_context);

            UserId = _userManager.GetUserId(this.User) ?? String.Empty;

            return Page();
        }

        [BindProperty]
        //public Models.Task Task { get; set; } = default!;
        public Create TaskCreate { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            TaskCreate.DateCreate = DateTime.Now;

            /* Add UserId into TaskCreate.UserId don't work - ModelState is invalid -> Error = "The UserId field is required."*/
            /* Why this is not working but the same with TaskCreate.DateCreate is ok?  */
            //TaskCreate.UserId = UserId;
            //TaskCreate.UserId = "d9b45e34-6014-4a42-b231-6c75686316d4";
            //TaskCreate.UserId = _userManager.GetUserId(this.User) ?? String.Empty;

            if (!ModelState.IsValid || _context.Tasks == null || TaskCreate == null)
            {
                TaskPriorityDropDownList(_context);
                TaskDropDownList(_context);
                return Page();
            }

            // Convert ViewModel TaskCreate to entity model Task
            Models.Task task = new Models.Task
            {
                Name = TaskCreate.Name,
                TaskPriorityId = TaskCreate.TaskPriorityId,
                DateDeadline = TaskCreate.DateDeadline,
                TaskParentId = TaskCreate.TaskParentId,
                DateCreate = TaskCreate.DateCreate,
                UserId = TaskCreate.UserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
