using FinanceManagement.Models;
using Microsoft.EntityFrameworkCore;
    namespace FinanceManagement
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Expense> Expenses { get; set; }
            public DbSet<Budget> Budgets { get; set; }
            public DbSet<Loan> Loans { get; set; }
        }
    }