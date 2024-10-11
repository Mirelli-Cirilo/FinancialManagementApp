using FinancialManagementApp.Models;
using System.Text.Json.Serialization;

namespace FinancialManagementApp.DTOs
{
    public class UpdateTransactionDto
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int BudgetId { get; set; }
        [JsonIgnore]
        public Budget? Budget { get; set; }
        public string UserId { get; set; }
    }
}
