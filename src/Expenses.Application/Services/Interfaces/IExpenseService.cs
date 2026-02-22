using Expenses.Application.DTOs;

namespace Expenses.Application.Services.Interfaces;

public interface IExpenseService
{
    Task<PagedResultDto<ExpenseDto>> GetPagedAsync(GetExpensesRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExpenseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ExpenseDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ExpenseDto> CreateAsync(CreateExpenseDto dto, CancellationToken cancellationToken = default);
    Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
