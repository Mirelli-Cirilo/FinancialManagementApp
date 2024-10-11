using FinancialManagementApp.Models;
using FinancialManagementApp.DTOs;

namespace FinancialManagementApp.Services.Interfaces;
public interface IBudgetService 
{
    Task<bool>BudgetsExist();
    Task<IEnumerable<BudgetDetailsDto>> GetBudgetsAsync();
    Task<BudgetDetailsDto> GetBudgetByIdAsync(int id);
    Task<BudgetDetailsDto> AddBudgetAsync(CreateBudgetDto budget);
    Task<bool> DeleteBudgetAsync(int id);
    Task<BudgetDetailsDto> UpdateBudgetAsync(int Id, UpdateBudgetDto budget);
}
