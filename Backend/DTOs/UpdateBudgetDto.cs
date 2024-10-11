namespace FinancialManagementApp.DTOs
{
    public class UpdateBudgetDto
    {
        public string Title { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }
}
