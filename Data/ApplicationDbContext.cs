using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinancialManagementApp.Models;


namespace FinancialManagementApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    // Define as DbSet para as outras entidades do seu aplicativo, se houver
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Budget> Budgets { get; set; }
}
