using FinancialManagementApp.Models;

namespace FinancialManagementApp.Services.Interfaces;

public interface IBudgetService 
{
    Task<IEnumerable<Budget>> GetBudgetsAsync();
    Task<Budget> GetBudgetByIdAsync(int id);
    Task AddBudgetAsync(Budget budget);
    Task DeleteBudgetAsync(int id);
    Task UpdateBudgetAsync(Budget budget);
}
