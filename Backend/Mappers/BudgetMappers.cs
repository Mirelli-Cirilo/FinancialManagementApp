using FinancialManagementApp.DTOs;
using FinancialManagementApp.Models;

namespace FinancialManagementApp.Mappers
{
    public static class BudgetMappers
    {
        public static BudgetDetailsDto ToBudgetDto(this Budget budget )
        {
            return new BudgetDetailsDto
            {
                Id = budget.Id,
                Title = budget.Title,
                InitialAmount = budget.InitialAmount,
                TotalAmount = budget.Amount,
                AmountSpent = budget.Transactions.Sum(t => t.Amount),
                TransactionCount = budget.Transactions.Count,
                UserId = budget.UserId
            };
        }

        public static Budget ToBudgetFromCreateDto(this CreateBudgetDto budgetDto)
        {
            return new Budget
            {
                Id = budgetDto.Id,
                Title = budgetDto.Title,
                InitialAmount = budgetDto.InitialAmount,
                Amount = budgetDto.InitialAmount,
                UserId = budgetDto.UserId
            };
        }

        public static Budget ToBudgetFromUpdateDto(this UpdateBudgetDto budgetDto)
        {
            return new Budget
            {
                Title = budgetDto.Title,
                InitialAmount = budgetDto.InitialAmount,
                Amount = budgetDto.Amount,
                UserId = budgetDto.UserId
            };
        }
    }
}
