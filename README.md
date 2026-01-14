# SimpleApi

SimpleApi is a sample RESTful API developed in ASP.NET Core 10, designed to simulate ticket and document management in a business environment. It uses a mock repository based on JSON files to simulate data access, following best practices in architecture, dependency injection, and structured logging.

## Main features

- Simulated ticket and document management
- Endpoints for querying, attaching, and updating tickets
- Decoupled and easy-to-test repository
- Professional and extensible structure
- Example integration with `.http` files for quick testing

## Technologies

- .NET 10
- ASP.NET Core Web API
- Dependency injection
- Structured logging
- Data simulation with JSON

## Usage

1. Clone the repository
2. Run `dotnet run` in the project folder
3. Test the endpoints using the `SimpleApi.http` file or Swagger

## Structure

- `Controllers/` - API controllers
- `Models/` - Data models
- `Repositories/` - Simulated data access logic
- `Interfaces/` - Repository contracts
- `Data/` - JSON files with mock data

---

**This project is for educational and testing purposes only.**
