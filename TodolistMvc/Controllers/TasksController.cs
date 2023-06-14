using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodolistMvc.Models;

namespace TodolistMvc.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(int? taskPriorityId, string searchString)
        {
            var mvcTaskContext = await _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority).ToListAsync();

            if (taskPriorityId != null)
            {
                mvcTaskContext = mvcTaskContext.Where(t => t.TaskPriorityId == taskPriorityId).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                mvcTaskContext = mvcTaskContext.Where(t => t.Name.Contains(searchString)).ToList();
            }

            mvcTaskContext.OrderBy(t => t.DateDeadline != null ? t.DateDeadline : DateTime.MaxValue).ThenBy(t => t.TaskPriorityId);

            //var mvcTaskContext = _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority)
            //    .OrderBy(t => t.DateDeadline != null ? t.DateDeadline : DateTime.MaxValue)
            //    .ThenBy(t => t.TaskPriorityId);

            ViewData["TaskPriorityId"] = new SelectList(_context.Set<TaskPriority>(), "Id", "Name");
            //ViewData["TaskPriorityIdFilter"] = taskPriorityId;
            //ViewData["TaskNameFilter"] = searchString;

            return View(mvcTaskContext);
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
                .Include(t => t.TaskPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            return View(new TaskNew());
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TaskPriorityId,DateDeadline,TaskParentId")] Models.TaskNew taskNew)
        {
            if (ModelState.IsValid)
            {
                // Map from TaskNew model to Task model/entity for save to db
                var task = new Models.Task()
                {
                    Id = taskNew.Id,
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
            var taskEdit = new TaskEdit()
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TaskPriorityId,DateDeadline,TaskParentId")] Models.TaskEdit taskEdit)
        {
            if (id != taskEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
            _context.Task.Where(t => t.Id == id || t.TaskParentId == id).ToList().ForEach(x =>
            {
                x.IsDone = true;
                x.DateDone = DateTime.Now;
            });

            await _context.SaveChangesAsync();

            return View("Index", await _context.Task.Include(t => t.TaskParent).Include(t => t.TaskPriority).ToListAsync());
        }
    }
}
