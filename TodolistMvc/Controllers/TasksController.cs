using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodolistMvc.Models;
using TodolistMvc.Models.Tasks;

namespace TodolistMvc.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskContext _context;
        private IValidator<TaskNewDTO> _newValidator;
        private IValidator<TaskEditDTO> _editValidator;

        public TasksController(TaskContext context, IValidator<TaskNewDTO> newValidator, IValidator<TaskEditDTO> editValidator)
        {
            _context = context;
            _newValidator = newValidator;
            _editValidator = editValidator;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(int? taskPriorityId, string searchString)
        {
            var mvcTaskContext = _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority);

            if (taskPriorityId != null)
            {
                mvcTaskContext.Where(t => t.TaskPriorityId == taskPriorityId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                mvcTaskContext.Where(t => t.Name.Contains(searchString));
            }

            mvcTaskContext.OrderBy(t => t.DateDeadline != null ? t.DateDeadline : DateTime.MaxValue).ThenBy(t => t.TaskPriorityId);

            var tasks = await mvcTaskContext.Select(t =>
                new TaskDTO()
                {
                    Id = t.Id,
                    Name = t.Name,
                    TaskPriorityId = t.TaskPriorityId,
                    TaskPriority = t.TaskPriority,
                    DateCreate = t.DateCreate,
                    DateDeadline = t.DateDeadline,
                    DateDone = t.DateDone,
                    IsDone = t.IsDone,
                    TaskParentId = t.TaskParentId,
                    TaskParent = t.TaskParent
                }
            ).ToListAsync();

            //var mvcTaskContext = _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority)
            //    .OrderBy(t => t.DateDeadline != null ? t.DateDeadline : DateTime.MaxValue)
            //    .ThenBy(t => t.TaskPriorityId);

            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name");
            //ViewData["TaskPriorityIdFilter"] = taskPriorityId;
            //ViewData["TaskNameFilter"] = searchString;

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.TaskParent)
                .Include(t => t.TaskPriority).Select(t =>
                    new TaskDetailDTO()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        TaskPriorityId = t.TaskPriorityId,
                        TaskPriority = t.TaskPriority,
                        DateCreate = t.DateCreate,
                        DateDeadline = t.DateDeadline,
                        DateDone = t.DateDone,
                        IsDone = t.IsDone,
                        TaskParentId = t.TaskParentId,
                        TaskParent = t.TaskParent
                    }).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewData["TaskParentId"] = new SelectList(_context.Set<Models.Task>().Where(t=> t.TaskParentId == null), "Id", "Name");
            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name");
            return View(new TaskNewDTO());
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TaskPriorityId,DateDeadline,TaskParentId")] TaskNewDTO taskNew)
        {
            ValidationResult result = await _newValidator.ValidateAsync(taskNew);

            if (result.IsValid)
            {
                // Map from TaskNew model to Task model/entity for save to db
                var task = new Models.Task()
                {
                    Name = taskNew.Name,
                    TaskPriorityId = taskNew.TaskPriorityId,
                    DateCreate = DateTime.Now,
                    DateDeadline = taskNew.DateDeadline,
                    TaskParentId = taskNew.TaskParentId
                };

                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            ViewData["TaskParentId"] = new SelectList(_context.Set<Models.Task>().Where(t => t.TaskParentId == null), "Id", "Name", taskNew.TaskParentId);
            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name", taskNew.TaskPriorityId);
            return View(taskNew);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Map from Task model/entity to TaskEdit model
            var taskEdit = new TaskEditDTO()
            {
                Id = task.Id,
                Name = task.Name,
                TaskPriorityId = task.TaskPriorityId,
                DateDeadline = task.DateDeadline,
                TaskParentId = task.TaskParentId
            };

            ViewData["TaskParentId"] = new SelectList(_context.Set<Models.Task>().Where(t => t.TaskParentId == null && t.Id != id), "Id", "Name", task.TaskParentId);
            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name", task.TaskPriorityId);
            return View(taskEdit);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TaskPriorityId,DateCreate,DateDone,DateDeadline,IsDone,TaskParentId")] Models.Task task)
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TaskPriorityId,DateDeadline,TaskParentId")] TaskEditDTO taskEdit)
        {
            if (id != taskEdit.Id)
            {
                return NotFound();
            }

            ValidationResult result = await _editValidator.ValidateAsync(taskEdit);

            if (result.IsValid)
            {
                var task = await _context.Task.FindAsync(id);
                // Map from TaskEdit model to Task model/entity for save to db
                if (task != null)
                {
                    task.Id = taskEdit.Id;
                    task.Name = taskEdit.Name;
                    task.TaskPriorityId = taskEdit.TaskPriorityId; 
                    task.DateDeadline = taskEdit.DateDeadline;
                    task.TaskParentId = taskEdit.TaskParentId;

                    try
                    {
                        _context.Update(task);
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
                }
                
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            ViewData["TaskParentId"] = new SelectList(_context.Set<Models.Task>().Where(t => t.TaskParentId == null && t.Id != id), "Id", "Name", taskEdit.TaskParentId);
            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name", taskEdit.TaskPriorityId);
            return View(taskEdit);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.TaskParent)
                .Include(t => t.TaskPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'MvcTaskContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);

            if (task != null)
            {
                // edit subordinate tasks - remove link to parent
                _context.Task.Where(t => t.TaskParentId == task.Id).ToList().ForEach(x =>
                {
                    x.TaskParentId = null;
                });
                _context.Task.Remove(task);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
          return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Remove all tasks
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("Clear")]
        public async Task<IActionResult> Clear()
        {
            // remove task and all his subordinate
            var removeTasks = await _context.Task.ToListAsync();
            if (removeTasks.Count() > 0)
            {
                _context.Task.RemoveRange(removeTasks);
            }

            await _context.SaveChangesAsync();

            //var mvcTaskContext = _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority);
            return View("Index", await _context.Task.ToListAsync());
        }

        /// <summary>
        /// Mark task and all his subordinates as done
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Done")]
        public async Task<IActionResult> Done(int id)
        {
            var tasksEditDone = await _context.Task.Where(t => t.Id == id || t.TaskParentId == id).Select (t => 
                new TaskEditDoneDTO()
                {
                    Id = t.Id,
                    DateDone = t.DateDone,
                    IsDone = t.IsDone
                }).ToListAsync();

            tasksEditDone.ForEach(t =>
            {
                t.IsDone = true;
                t.DateDone = DateTime.Now;
            });

            foreach(var taskEditDone in tasksEditDone)
            {
                var task = await _context.Task.FindAsync(taskEditDone.Id);

                if (task != null)
                {
                    task.DateDone = taskEditDone.DateDone;
                    task.IsDone = taskEditDone.IsDone;
                    try
                    {
                        _context.Update(task);
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
                }
            }

            await _context.SaveChangesAsync();

            var tasks = await _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority).Select(t =>
                new TaskDTO()
                {
                    Id = t.Id,
                    Name = t.Name,
                    TaskPriorityId = t.TaskPriorityId,
                    TaskPriority = t.TaskPriority,
                    DateCreate = t.DateCreate,
                    DateDeadline = t.DateDeadline,
                    DateDone = t.DateDone,
                    IsDone = t.IsDone,
                    TaskParentId = t.TaskParentId,
                    TaskParent = t.TaskParent
                }
            ).ToListAsync();

            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name");

            return View("Index", tasks);
        }
    }
}
