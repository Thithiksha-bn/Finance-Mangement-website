using Microsoft.AspNetCore.Http;
using FinanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return 0;
            return userId.Value;
        }

        // GET: Budgets/Index
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return View(budgets);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Budgets/Create
        [HttpPost]
        public async Task<IActionResult> Create(Budget budget)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                budget.UserId = userId;
                budget.SpentAmount = 0;
                budget.CreatedDate = DateTime.Now;
                _context.Budgets.Add(budget);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null || budget.UserId != userId)
                return NotFound();

            return View(budget);
        }

        // POST: Budgets/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Budget budget)
        {
            if (id != budget.Id)
                return NotFound();

            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var existingBudget = await _context.Budgets.FindAsync(id);
                if (existingBudget == null || existingBudget.UserId != userId)
                    return NotFound();

                existingBudget.Category = budget.Category;
                existingBudget.MonthlyBudget = budget.MonthlyBudget;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null || budget.UserId != userId)
                return NotFound();

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var budget = await _context.Budgets.FindAsync(id);
            if (budget != null && budget.UserId == userId)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpGet("/api/budgets/user/{userId}")]
        public async Task<IActionResult> GetUserBudgetsApi(int userId)
        {
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();
            return Ok(budgets);
        }
    }
}