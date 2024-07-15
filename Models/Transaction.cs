using System;
using Microsoft.AspNetCore.Identity;

namespace FinancialManagementApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string UserId{ get; set; }
    public ApplicationUser? User { get; set; }
}
