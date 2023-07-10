using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneProjectToDoApplication.Database.DbConnection;
using CapstoneProjectToDoApplication.Models;
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable S2259

namespace CapstoneProjectToDoApplication.Controllers
{
    public class ToDoListsController : Controller
    {
        private readonly ToDoDbContext _context;

        public ToDoListsController(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = _context.ToDoLists.Where(l => l.Hide != true);
            return View(await result.ToListAsync());
        }

        public async Task<IActionResult> Copy(int id)
        {
            var list = await _context.ToDoLists.SingleOrDefaultAsync(list => list.Id == id);
            ToDoList result = new()
            {
                Title = "Copy "+ list.Title,
                Hide = false,
            };

            _context.ToDoLists.Add(result);

            var tasks = _context.ToDoTasks.Where(x => x.ListId == id).ToList();

            foreach (var task in tasks)
            {
                var newTask = new ToDoTask
                {
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    Created = task.Created,
                    DueDate = task.DueDate,
                    IsCompleted = task.IsCompleted,
                    ListId = result.Id,
                    List = result
                };

                _context.ToDoTasks.Add(newTask);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowHidden()
        {
            var result = _context.ToDoLists.Where(l => l.Hide == true);
            return View(await result.ToListAsync());
        }

        public async Task<IActionResult> Hide(int id)
        {
            ToDoList list = _context.ToDoLists.FirstOrDefault(x => x.Id == id);
            list.Hide = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Unhide(int id)
        {
            ToDoList list = _context.ToDoLists.FirstOrDefault(x => x.Id == id);
            list.Hide = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var toDoList = await _context.ToDoLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        public IActionResult Create()
        {
            return View(new ToDoList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ToDoList toDoList)
        {
            _context.Add(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            return View(toDoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] ToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(toDoList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoListExists(toDoList.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoLists == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoLists == null)
            {
                return Problem("Entity set 'ToDoDbContext.ToDoLists'  is null.");
            }
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList != null)
            {
                _context.ToDoLists.Remove(toDoList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoLists.Any(e => e.Id == id);
        }
    }
}
