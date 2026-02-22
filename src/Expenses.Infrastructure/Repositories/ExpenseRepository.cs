using Expenses.Domain.Entities;
using Expenses.Domain.Interfaces;
using Expenses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;

    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Expense> Items, int TotalCount)> GetPagedAsync(string category, DateOnly? occurredOnFrom, DateOnly? occurredOnTo, int skip, int take, CancellationToken cancellationToken = default)
    {
        var query = _context.Expenses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(e => e.Category.Contains(category));
        if (occurredOnFrom.HasValue)
            query = query.Where(e => e.OccurredOn >= occurredOnFrom.Value);
        if (occurredOnTo.HasValue)
            query = query.Where(e => e.OccurredOn <= occurredOnTo.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(e => e.OccurredOn)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Expense>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .OrderByDescending(e => e.OccurredOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Expense> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Expense> AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);
        return expense;
    }

    public async Task UpdateAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
