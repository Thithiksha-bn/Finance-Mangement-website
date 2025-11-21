using Microsoft.AspNetCore.Http;
using FinanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "All fields are required" });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            user.CreatedDate = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Signup successful! Please login.", userId = user.Id });
        }

        // GET: Users/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
                return BadRequest(new { message = "Invalid credentials" });

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return Ok(new { userId = user.Id, username = user.Username });
        }

        // GET: Users/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}