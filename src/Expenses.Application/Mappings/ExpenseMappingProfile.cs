using AutoMapper;
using Expenses.Application.DTOs;
using Expenses.Domain.Entities;

namespace Expenses.Application.Mappings;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<CreateExpenseDto, Expense>();
        CreateMap<UpdateExpenseDto, Expense>();
    }
}
