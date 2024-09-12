using FinancialManagementApp.Models;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;
public interface IBudgetService 
{
    Task<IEnumerable<BudgetDetailsDto>> GetBudgetsAsync();
    Task<BudgetDetailsDto> GetBudgetByIdAsync(int id);
    Task<Budget> AddBudgetAsync(Budget budget);
    Task DeleteBudgetAsync(int id);
    Task<Budget> UpdateBudgetAsync(int Id, BudgetDetailsDto budget);
}
