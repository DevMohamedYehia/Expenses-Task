using AutoMapper;
using Expenses.Application.DTOs;
using Expenses.Application.Services.Interfaces;
using Expenses.Domain.Constants;
using Expenses.Domain.Entities;
using Expenses.Domain.Interfaces;

namespace Expenses.Application.Services.Implementations;

public class ExpenseService : IExpenseService
{

    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public ExpenseService(IExpenseRepository repository, IMapper mapper, ICurrentUser currentUser)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PagedResultDto<ExpenseDto>> GetPagedAsync(GetExpensesRequest request, CancellationToken cancellationToken = default)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
        if (pageSize > ExpenseConstants.MaxPageSize)
            pageSize = ExpenseConstants.MaxPageSize;

        var skip = (pageNumber - 1) * pageSize;
        var (items, totalCount) = await _repository.GetPagedAsync(
            request.Category,
            request.OccurredOnFrom,
            request.OccurredOnTo,
            skip,
            pageSize,
            cancellationToken);

        return new PagedResultDto<ExpenseDto>
        {
            Items = _mapper.Map<IReadOnlyList<ExpenseDto>>(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IReadOnlyList<ExpenseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var expenses = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ExpenseDto>>(expenses);
    }

    public async Task<ExpenseDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var expense = await _repository.GetByIdAsync(id, cancellationToken);
        return expense is null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<ExpenseDto> CreateAsync(CreateExpenseDto dto, CancellationToken cancellationToken = default)
    {
        var expense = _mapper.Map<Expense>(dto);
        expense.CreatedByUserId = _currentUser.GetId();
        expense.CreatedAt = DateTime.UtcNow;

        var created = await _repository.AddAsync(expense, cancellationToken);
        return _mapper.Map<ExpenseDto>(created);
    }

    public async Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto, CancellationToken cancellationToken = default)
    {
        var expense = await _repository.GetByIdAsync(id, cancellationToken);
        if (expense is null) return null;

        _mapper.Map(dto, expense);
        expense.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(expense, cancellationToken);
        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var expense = await _repository.GetByIdAsync(id, cancellationToken);
        if (expense is null) return false;

        await _repository.DeleteAsync(expense, cancellationToken);
        return true;
    }
}
