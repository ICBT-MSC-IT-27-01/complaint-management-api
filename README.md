# Complaint Management System (CMS) API
.NET 8 | ADO.NET + Stored Procedures | JWT Auth | xUnit Tests

## Quick Start
1. Run SQL/Setup/00_CreateTables.sql on your MSSQL database
2. Run all SPs in SQL/*/ folders
3. Update Cd.Cms.Api/appsettings.json (connection string + JWT key)
4. dotnet build && dotnet run --project Cd.Cms.Api
5. Open https://localhost:7000/swagger

## Projects
- Cd.Cms.Api          - Controllers, Program.cs
- Cd.Cms.Application  - Services, Interfaces, DTOs
- Cd.Cms.Infrastructure - Repositories (ADO.NET + SPs)
- Cd.Cms.Domain       - Domain entities
- Cd.Cms.Shared       - ApiResponse, DataReader, JwtSettings
- Cd.Cms.Tests        - xUnit + Moq unit tests
