using FinancialManagementApp.Data;
using FinancialManagementApp.DTOs;
using FinancialManagementApp.Mappers;
using FinancialManagementApp.Models;
using FinancialManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<bool> BudgetsExist()
    {

        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null)
        {
            throw new Exception("User not authenticated.");
        }
        var budgetExist = await _context.Budgets.AnyAsync(b => b.UserId == userId);
        return budgetExist;
    }

    public async Task<BudgetDetailsDto> AddBudgetAsync(CreateBudgetDto budgetDto)
    {
        var budgetModel = budgetDto.ToBudgetFromCreateDto();

        _context.Budgets.Add(budgetModel);
        await _context.SaveChangesAsync();

        return budgetModel.ToBudgetDto();
    }

    public async Task<BudgetDetailsDto> GetBudgetByIdAsync(int id)
    {

        var budget = await _context.Budgets
        .Include(b => b.Transactions)
        .FirstOrDefaultAsync(b => b.Id == id);

        return budget.ToBudgetDto();
    }

    public async Task<IEnumerable<BudgetDetailsDto>> GetBudgetsAsync()
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var budgets = await _context.Budgets
            .Include(b => b.Transactions)
            .Where(b => b.UserId == userId)
            .Select(b => b.ToBudgetDto())
            .ToListAsync(); 

        return budgets;
    }

    public async Task<BudgetDetailsDto> UpdateBudgetAsync(int Id, UpdateBudgetDto budget)
    {
        var existingBudget = _context.Budgets.FirstOrDefault(x => x.Id == Id);
        if (existingBudget == null)
        {
            throw new Exception("Budget not found.");
        }

        existingBudget.Title = budget.Title;
        existingBudget.Amount = budget.InitialAmount;
        existingBudget.InitialAmount = budget.InitialAmount;
   

        _context.Budgets.Update(existingBudget);
        await _context.SaveChangesAsync();

        return existingBudget.ToBudgetDto();
    }

    public async Task<bool> DeleteBudgetAsync(int id)
    {
        var budget = _context.Budgets.FirstOrDefault(x => x.Id == id);
        if (budget != null)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    
}
