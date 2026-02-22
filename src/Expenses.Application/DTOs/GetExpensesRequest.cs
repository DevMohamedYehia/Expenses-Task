namespace Expenses.Application.DTOs;

public class GetExpensesRequest
{
    public string Category { get; set; }
    public DateOnly? OccurredOnFrom { get; set; }
    public DateOnly? OccurredOnTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
