# Questionnaire API

This is a .NET 8 Web API for creating and managing questionnaires and collecting responses.

It supports multiple types of questions, allows users to submit responses, and stores data in JSON files. The project includes both unit and integration tests.

---

##  Features

Create questionnaires with different question types:  
5-Star Rating (customizable range from 1 to 10)  
Multi-Select (choose multiple options)  
Single-Select (choose only one option)  

Submit responses to questions  
Retrieve a question by ID  
Retrieve all questions (optional)  
Fully file-based (no database)  
Supports unit and integration testing via `xUnit`

---

## Technologies

- .NET 8 (ASP.NET Core)
- C#
- Newtonsoft.Json for polymorphic serialization
- `xUnit` + `FluentAssertions` for testing
- `Microsoft.AspNetCore.Mvc.Testing` for integration tests
- File-based persistence

---

## Project Structure
This project follows a clean architecture approach with separation of concerns:
Core: Contracts, enums, and base types
Infrastructure: File-based persistence and implementation of services
WepApi: The API host, routing, and controllers
Tests: Unit and integration tests

To support integration testing:

The Program.cs file includes a public Program class.
WebApplicationFactory<Program> is used to create a testable server.
Files are handled with temporary or custom paths inside tests.