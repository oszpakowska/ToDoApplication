using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneProjectToDoApplication.Database.DbConnection;
using CapstoneProjectToDoApplication.Models;
#pragma warning disable S2259
#pragma warning disable CS8600
#pragma warning disable CS8602

namespace CapstoneProjectToDoApplication.Controllers
{
    public class ToDoTasksController : Controller
    {
        private readonly ToDoDbContext _context;
        private readonly int date = DateTime.Today.DayOfYear;

        public ToDoTasksController(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var toDoDbContext = _context.ToDoTasks.Where(x => x.IsCompleted != true);
            return View(await toDoDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetTasks(int id)
        {
            var tasks = _context.ToDoTasks.Where(x => x.ListId == id).Where(x => x.IsCompleted != true);
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> CompletedTasks(int id)
        {
            var tasks = _context.ToDoTasks.Where(x => x.ListId == id).Where(x => x.IsCompleted == true);
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> CompletedTasksAll()
        {
            var tasks = _context.ToDoTasks.Where(x => x.IsCompleted == true);
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> TasksDueDateToday()
        {
            var tasks = _context.ToDoTasks.Where(x => x.DueDate.DayOfYear == date).Where(x => x.Status != "Done");
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var toDoTask = await _context.ToDoTasks
                .Include(t => t.List)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        public IActionResult Create()
        {
            ViewData["ListId"] = new SelectList(_context.ToDoLists, "Id", "Title");
            return View(new ToDoTask());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,Created,DueDate,IsCompleted,ListId")] ToDoTask toDoTask)
        {
            _context.Add(toDoTask);
            toDoTask.Created = DateTime.Now;
            toDoTask.Status = "Not started";
            toDoTask.IsCompleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }
            ViewData["ListId"] = new SelectList(_context.ToDoLists, "Id", "Title", toDoTask.ListId);
            return View(toDoTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Title,Description,Notes,Status,Created,DueDate,IsCompleted,ListId")] ToDoTask toDoTask)
        {
            var result = _context.ToDoTasks.SingleOrDefault(t => t.Id == id);
            if (result != null)
            {
                result.Title = toDoTask.Title;
                result.Description = toDoTask.Description;
                result.Status = toDoTask.Status;
                result.DueDate = toDoTask.DueDate;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var toDoTask = await _context.ToDoTasks
                .Include(t => t.List)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoTasks == null)
            {
                return Problem("Entity set 'ToDoDbContext.ToDoTasks'  is null.");
            }
            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            if (toDoTask != null)
            {
                _context.ToDoTasks.Remove(toDoTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Done(int id)
        {
            ToDoTask task = _context.ToDoTasks.FirstOrDefault(x => x.Id == id);
            task.IsCompleted = true;
            task.Status = "Done";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CompletedTasksAll));
        }
    }
}
