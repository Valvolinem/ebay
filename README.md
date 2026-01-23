# eBay Automation Framework

A Selenium-based test automation framework for eBay using SpecFlow (BDD) and NUnit.

## Prerequisites

- .NET 6.0 SDK
- Visual Studio 2022 or Visual Studio Code
- Chrome/Edge browser

## Project Structure

```
├── Features/              # SpecFlow feature files (Gherkin scenarios)
├── StepDefinitions/       # Step definition implementations
├── Pages/                 # Page Object Model classes
│   ├── Components/        # Reusable page components
│   └── Constants/         # Page-level constants
├── Tests/                 # NUnit test classes
├── Hooks/                 # SpecFlow hooks (Before/After scenarios)
├── Utilities/             # Helper classes and utilities
└── Configuration/         # Configuration files
```

## Setup

1. Clone the repository
2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

## Running Tests

Execute all tests:
```bash
dotnet test
```

Build the project:
```bash
dotnet build
```

## Features

- **Page Object Model**: Organized page classes for maintainability
- **SpecFlow BDD**: Behavior-driven development with Gherkin syntax
- **Selenium WebDriver**: Browser automation
- **NUnit**: Test execution framework

## Test Scenarios

Current test scenarios cover:
- Product search functionality
- Product details viewing
- Shopping cart operations
