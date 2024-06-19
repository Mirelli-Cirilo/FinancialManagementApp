using System;
using Microsoft.AspNetCore.Identity;

namespace FinancialManagementApp.Models;

public class Budget
{
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal Limit{ get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
}
