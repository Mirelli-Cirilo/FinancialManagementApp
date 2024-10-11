namespace FinancialManagementApp.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int BudgetId { get; set; }
    public string UserId { get; set; }
}