using System;
using System.Text.Json.Serialization;
namespace FinancialManagementApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string UserId{ get; set; }
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public int BudgetId { get; set; }
    [JsonIgnore]
    public Budget Budget { get; set; }
    
}
