using Microsoft.AspNetCore.Http;
using FinanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Controllers
{
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
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

        // GET: Loans/Index
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var loans = await _context.Loans
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return View(loans);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Loans/Create
        [HttpPost]
        public async Task<IActionResult> Create(Loan loan)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                loan.UserId = userId;
                loan.PaidAmount = 0;
                loan.CreatedDate = DateTime.Now;
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Login", "Users");

            if (id == null)
                return NotFound();

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null || loan.UserId != userId)
                return NotFound();

            return View(loan);
        }

        // POST: Loans/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Loan loan)
        {
            if (id != loan.Id)
                return NotFound();

            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var existingLoan = await _context.Loans.FindAsync(id);
                if (existingLoan == null || existingLoan.UserId != userId)
                    return NotFound();

                existingLoan.LenderName = loan.LenderName;
                existingLoan.LoanType = loan.LoanType;
                existingLoan.Principal = loan.Principal;
                existingLoan.MonthlyEMI = loan.MonthlyEMI;
                existingLoan.PaidAmount = loan.PaidAmount;
                existingLoan.StartDate = loan.StartDate;
                existingLoan.EndDate = loan.EndDate;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null || loan.UserId != userId)
                return NotFound();

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int userId = GetUserId();
            if (userId == 0)
                return RedirectToAction("Index", "Home");

            var loan = await _context.Loans.FindAsync(id);
            if (loan != null && loan.UserId == userId)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpGet("/api/loans/user/{userId}")]
        public async Task<IActionResult> GetUserLoansApi(int userId)
        {
            var loans = await _context.Loans
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();
            return Ok(loans);
        }
    }
}