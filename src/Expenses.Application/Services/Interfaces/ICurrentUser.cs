namespace Expenses.Application.Services.Interfaces;

public interface ICurrentUser
{
    /// <summary>
    /// Gets the current user's id from the request (e.g. JWT claims).
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the user is not authenticated or the user id claim is missing.</exception>
    string GetId();
}
