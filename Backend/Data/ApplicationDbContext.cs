
using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Models;

namespace FinancialManagementApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }

    
}