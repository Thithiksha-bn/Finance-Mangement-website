using Microsoft.AspNetCore.Http;
using FinanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
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

        // GET: Expenses/Index
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                expense.UserId = userId;
                expense.CreatedDate = DateTime.Now;
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();

                // Update budget spent amount
                var budget = await _context.Budgets.FirstOrDefaultAsync(b =>
                    b.UserId == userId && b.Category == expense.Category);
                if (budget != null)
                {
                    budget.SpentAmount += expense.Amount;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.UserId != userId)
                return NotFound();

            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id)
                return NotFound();

            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var existingExpense = await _context.Expenses.FindAsync(id);
                if (existingExpense == null || existingExpense.UserId != userId)
                    return NotFound();

                existingExpense.Title = expense.Title;
                existingExpense.Category = expense.Category;
                existingExpense.Amount = expense.Amount;
                existingExpense.ExpenseDate = expense.ExpenseDate;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.UserId != userId)
                return NotFound();

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null && expense.UserId == userId)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpGet("/api/expenses/user/{userId}")]
        public async Task<IActionResult> GetUserExpensesApi(int userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
            return Ok(expenses);
        }
    }
}