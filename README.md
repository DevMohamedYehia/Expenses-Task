Expenses Task

Overview

* I chose to use Clean Architecture.
* I chose to build the project using ASP.NET Core Identity and JWT for authentication and authorization.
* I chose to use the Repository Pattern for data access.
* I chose to keep everything simple for now (no generic components).
* I chose to use FluentValidation to validate DTOs.
* I chose to use AutoMapper to map between Entities and DTOs.

How to start the application

1. Clone the repository.
2. Run deploy/InitialCreate.sql to create the database and tables.
3. Run the startup project: Expenses.API
4. Use the Register API to create a user.
5. Use the Login API to get a valid JWT token.
6. Use this JWT to create/edit/update/delete Expenses:
   * In Swagger, click Authorize and paste the token: <your_token>
