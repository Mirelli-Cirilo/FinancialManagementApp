using System;
namespace FinancialManagementApp.Models;

public class Budget
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal Amount{ get; set; }
    public string UserId{ get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    
}
