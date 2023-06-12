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

namespace Todolist.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SelectList TaskPrioritySL { get; set; }

        public CreateModel(Todolist.Data.TodolistContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void TaskPriorityDropDownList(TodolistContext _context,
            object selectedTaskPriority = null)
        {
            var taskPriorityQuery = from p in _context.TaskPriority
                                        //orderby d.Name // Sort by name.
                                    select p;

            TaskPrioritySL = new SelectList(taskPriorityQuery.AsNoTracking(),
                nameof(TaskPriority.Id),
                nameof(TaskPriority.Name),
                selectedTaskPriority);
        }

        public IActionResult OnGet()
        {

            TaskPriorityDropDownList(_context);

            //tp = (List<TaskPriority>)_context.TaskPriority.Select(x => x.Id);

            //TaskPriorities = _context.TaskPriority.Select(p =>
            //                      new SelectListItem
            //                      {
            //                          Value = p.Id.ToString(),
            //                          Text = p.Name
            //                      })
            //                      .ToList();

            //TaskPriorities.Insert(0, new SelectListItem()
            //{
            //    Value = "",
            //    Text = "--- Select ---"
            //});

            

            //Tasks = _context.Tasks.Where(x => x.TaskParent == null && x.IsDone == false)
            //    .Select(t =>
            //                      new SelectListItem
            //                      {
            //                          Value = t.Id.ToString(),
            //                          Text = t.Name
            //                      }).ToList();

            //Tasks.Insert(0, new SelectListItem()
            //{
            //    Value = "",
            //    Text = "--- No parent task ---"
            //});

            return Page();
        }

        [BindProperty]
        public Models.Task Task { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Task.DateCreate = DateTime.Now;
            Task.User = await _userManager.GetUserAsync(this.User);
            var y = _context.TaskPriority.Where(x => x.Id == 1);

            if (!ModelState.IsValid || _context.Tasks == null || Task == null)
            {
                return Page();
            }

            _context.Tasks.Add(Task);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
