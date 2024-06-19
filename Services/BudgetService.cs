using FinancialManagementApp.Data;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementApp.Services;

public class BudgetService : IBudgetService
{
    private readonly ApplicationDbContext _context;
    public BudgetService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddBudgetAsync(Budget budget)
    {
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBudgetAsync(int id)
    {
        var budget = await _context.Budgets.FindAsync(id);
        if (budget != null)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }
        
    }

    public async Task<Budget> GetBudgetByIdAsync(int id)
    {
        return await _context.Budgets.FindAsync(id);
    }

    public async Task<IEnumerable<Budget>> GetBudgetsAsync()
    {
        return await _context.Budgets.ToListAsync();
    }

    public async Task UpdateBudgetAsync(Budget budget)
    {
        _context.Budgets.Update(budget);
        await _context.SaveChangesAsync();
    }
}
