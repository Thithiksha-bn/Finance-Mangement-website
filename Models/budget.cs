namespace FinanceManagement.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Category { get; set; }
        public decimal MonthlyBudget { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}