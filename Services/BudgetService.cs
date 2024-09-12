using FinancialManagementApp.Data;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; 

namespace FinancialManagementApp.Services;

public class BudgetService : IBudgetService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BudgetService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Budget> AddBudgetAsync(Budget budget)
    {
        var createBudget = new Budget
        {
            Id = 0,
            Title = budget.Title,
            InitialAmount = budget.InitialAmount,
            Amount = budget.InitialAmount,
            UserId = budget.UserId 
        };

        _context.Budgets.Add(createBudget);
        await _context.SaveChangesAsync();
        return createBudget;
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

    public async Task<BudgetDetailsDto> GetBudgetByIdAsync(int id)
    {

        var budget = await _context.Budgets.Include(b => b.Transactions).FirstOrDefaultAsync(b => b.Id == id);

        if(budget == null)
        {
            return null;
        }

        var result = new BudgetDetailsDto
        {
            Id = budget.Id,
            Title = budget.Title,
            UserId = budget.UserId,
            TotalAmount = budget.Amount,
            InitialAmount = budget.InitialAmount,
            AmountSpent = budget.Transactions.Sum(t => t.Amount),
            TransactionCount = budget.Transactions.Count()
        };
        return result;
    }

    public async Task<IEnumerable<BudgetDetailsDto>> GetBudgetsAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        var budgets = await _context.Budgets
            .Include(b => b.Transactions)
            .Where(b => b.UserId == userId)
            .ToListAsync(); 

        

        var result = budgets.Select(b => new BudgetDetailsDto
        {
            Id = b.Id,
            Title = b.Title,
            UserId = b.UserId,
            TotalAmount = b.Amount,
            InitialAmount = b.InitialAmount,
            AmountSpent = b.Transactions.Sum(t => t.Amount),
            TransactionCount = b.Transactions.Count()
        });

        return result;
    }

    public async Task<Budget> UpdateBudgetAsync(int Id, BudgetDetailsDto budget)
    {

        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var existingBudget = await _context.Budgets.FindAsync(Id);
        if (existingBudget == null)
        {
            throw new Exception("Budget not found.");
        }

        existingBudget.Title = budget.Title;
        existingBudget.Amount = budget.InitialAmount;
        existingBudget.InitialAmount = budget.InitialAmount;
        existingBudget.UserId = userId;

        _context.Budgets.Update(existingBudget);
        await _context.SaveChangesAsync();

        return existingBudget;
    }
}
