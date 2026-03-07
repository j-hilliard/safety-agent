# API Guidelines

## Table of Contents
1. [Project Structure](#project-structure)
2. [Controllers](#controllers)
3. [Models & DTOs](#models--dtos)
4. [Domain Layer](#domain-layer)
5. [Services](#services)
6. [Validation](#validation)
7. [Data Access](#data-access)
8. [Configuration & Settings](#configuration--settings)
9. [Authorization & Security](#authorization--security)
10. [Shared Code & Constants](#shared-code--constants)
11. [Best Practices](#best-practices)
12. [Resources](#resources)

---

## 1. Project Structure

- **Api/**: Main ASP.NET Core Web API project (controllers, services, validators, helpers, configuration, etc.)
- **Data/**: Data access layer (DbContext, migrations, data models, initialization)
- **Shared/**: Shared code (constants, enums, attributes) used across projects

---

## 2. Controllers

- Place all API controllers in `Api/Controllers/`.
- Use `[ApiController]` and route attributes for clarity and consistency.
- Keep controllers thin: delegate business logic to services.
- Return appropriate HTTP status codes and use IActionResult or generics (e.g., `ActionResult<T>`).
- Group endpoints logically by resource/domain.

---

## 3. Models & DTOs

- Place API models and DTOs in `Api/Models/` or `Data/Models/` as appropriate.
- Use DTOs to shape data for requests and responses; avoid exposing EF entities directly.
- Use clear, descriptive names (e.g., `UserDto`, `CreateProjectRequest`).

---

## 4. Domain Layer

- Place domain logic and entities in `Api/Domain/`.
- Encapsulate business rules and domain behaviors here.
- Keep domain models free of infrastructure concerns.

---

## 5. Services

- Place business logic and orchestration in `Api/Services/`.
- Use dependency injection for service registration.
- Keep services focused and single-responsibility.

---

## 6. Validation

- Place validation logic in `Api/Validators/`.
- Use FluentValidation or data annotations for model validation.
- Validate all incoming data at the API boundary.

---

## 7. Data Access

- Place EF Core DbContext and related logic in `Data/AppDbContext.cs`.
- Use `Data/Migrations/` for EF Core migrations.
- Place data models in `Data/Models/`.
- Use repositories or direct DbContext access as appropriate for your project.

---

## 8. Configuration & Settings

- Place configuration classes in `Api/Configuration/`.
- Use `appsettings.json` and environment-specific files for configuration.
- Use strongly-typed configuration objects where possible.

---

## 9. Authorization & Security

- Place authorization logic in `Api/Authorization/`.
- Use ASP.NET Core's built-in authentication/authorization mechanisms.
- Secure all endpoints appropriately (e.g., `[Authorize]` attributes).
- Never log or expose sensitive information.

---

## 10. Shared Code & Constants

- Place shared constants, enums, and attributes in `Shared/`.
- Use `Shared/Constants.cs` for application-wide constants.
- Use `Shared/Enumerations/` for enums and value objects.
- Use `Shared/Attributes/` for custom attributes.

---

## 11. Best Practices

- **Separation of Concerns:** Keep controllers, services, domain, and data access logic separate.
- **Error Handling:** Use global exception handling middleware. Return meaningful error messages.
- **Async/Await:** Use async methods for I/O-bound operations.
- **Testing:** Write unit and integration tests for controllers, services, and data access.
- **Documentation:** Use XML comments and tools like Swagger/NSwag for API documentation.
- **Code Style:** Follow C# and .NET naming conventions. Use consistent formatting and linting.

---

## 12. Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag)
- [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines)

---

**For questions or to propose changes to these guidelines, please open a pull request or contact the project maintainers.**
