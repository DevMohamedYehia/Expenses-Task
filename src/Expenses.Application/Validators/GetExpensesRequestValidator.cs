using Expenses.Application.DTOs;
using Expenses.Domain.Constants;
using FluentValidation;

namespace Expenses.Application.Validators;

public class GetExpensesRequestValidator : AbstractValidator<GetExpensesRequest>
{

    public GetExpensesRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, ExpenseConstants.MaxPageSize).WithMessage($"PageSize must be between 1 and {ExpenseConstants.MaxPageSize}.");

        RuleFor(x => x)
            .Must(x => !x.OccurredOnFrom.HasValue || !x.OccurredOnTo.HasValue || x.OccurredOnFrom <= x.OccurredOnTo)
            .WithMessage("OccurredOnFrom must be less than or equal to OccurredOnTo.");
    }
}
