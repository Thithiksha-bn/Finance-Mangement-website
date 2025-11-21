namespace FinanceManagement.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string LenderName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LoanType { get; set; }
        public decimal Principal { get; set; }
        public decimal MonthlyEMI { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}