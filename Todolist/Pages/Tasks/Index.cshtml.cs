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
    public class IndexModel : PageModel
    {
        private readonly Todolist.Data.TodolistContext _context;

        public IndexModel(Todolist.Data.TodolistContext context)
        {
            _context = context;
        }

        public IList<Models.Task> Task { get;set; } = default!;

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            if (_context.Tasks != null)
            {
                Task = await _context.Tasks.ToListAsync();
            }
        }
    }
}
