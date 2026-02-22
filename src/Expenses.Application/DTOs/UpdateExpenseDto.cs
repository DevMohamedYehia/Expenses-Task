namespace Expenses.Application.DTOs;

public class UpdateExpenseDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateOnly OccurredOn { get; set; }
}
