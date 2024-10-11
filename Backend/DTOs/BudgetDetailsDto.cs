using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialManagementApp.DTOs
{
    public class BudgetDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountSpent { get; set; }
    public int TransactionCount { get; set; }
    public string UserId { get; set; }
}
}