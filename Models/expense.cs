namespace FinanceManagement.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}