using Expenses.Application.DTOs;
using Expenses.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IValidator<CreateExpenseDto> _createExpenseValidator;
    private readonly IValidator<UpdateExpenseDto> _updateExpenseValidator;
    private readonly IValidator<GetExpensesRequest> _getExpensesRequestValidator;

    public ExpensesController(
        IExpenseService expenseService,
        IValidator<CreateExpenseDto> createExpenseValidator,
        IValidator<UpdateExpenseDto> updateExpenseValidator,
        IValidator<GetExpensesRequest> getExpensesRequestValidator)
    {
        _expenseService = expenseService;
        _createExpenseValidator = createExpenseValidator;
        _updateExpenseValidator = updateExpenseValidator;
        _getExpensesRequestValidator = getExpensesRequestValidator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetExpensesRequest request, CancellationToken cancellationToken)
    {
        request.PageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        request.PageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var validationResult = await _getExpensesRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

        var result = await _expenseService.GetPagedAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var expense = await _expenseService.GetByIdAsync(id, cancellationToken);
        return expense is null ? NotFound() : Ok(expense);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateExpenseDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createExpenseValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

        var created = await _expenseService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseDto dto, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var validationResult = await _updateExpenseValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

        var updated = await _expenseService.UpdateAsync(id, dto, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var deleted = await _expenseService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
