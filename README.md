# TestFlow – C# Test Automation Platform

A lightweight test automation platform built with C# and .NET 10.

The project demonstrates object-oriented programming, interface-based design, automated test execution, exception handling, JSON reporting, and unit testing with xUnit.

## Features

- **Multiple test types:** Supports range-based measurements and connection tests.
- **Interface-based architecture:** Uses `ITestCase` to support different test implementations.
- **Automated execution:** Executes multiple test cases through a common `TestRunner`.
- **Exception isolation:** A configuration error in one test does not prevent other tests from running.
- **Result tracking:** Classifies individual tests as `Passed`, `Failed`, or `Error`.
- **Test summary:** Calculates total, passed, failed, and error counts, along with the overall test suite status.
- **JSON reporting:** Exports structured test results to `test-report.json`.
- **Unit testing:** Uses xUnit to verify core application behavior.

## Technology Stack

- C#
- .NET 10
- xUnit
- System.Text.Json
- Git / GitHub

## Project Structure

```text
TestFlow/
├── Models/
│   ├── ITestCase.cs
│   ├── RangeTest.cs
│   ├── ConnectionTest.cs
│   ├── TestExecutionResult.cs
│   ├── TestExecutionStatus.cs
│   ├── TestSummary.cs
│   └── TestSuiteStatus.cs
├── Services/
│   └── TestRunner.cs
├── Reporting/
│   └── JsonReportExporter.cs
├── TestFlow.Tests/
│   ├── RangeTestTests.cs
│   ├── TestRunnerTests.cs
│   └── JsonReportExporterTests.cs
├── Program.cs
├── TestFlow.csproj
└── README.md
```

## Architecture

The application separates test definitions, execution, result aggregation, and reporting.

**1. Test Definitions**

The `ITestCase` interface defines the common contract for executable tests:

```csharp
public interface ITestCase
{
    string TestName { get; }
    bool IsPassed();
}
```

Two test types are implemented:

- `RangeTest`: Checks whether a measured value is within specified limits.
- `ConnectionTest`: Checks whether a device connection is successful.

New test types can be added by implementing `ITestCase`.

**2. Test Execution**

`TestRunner` executes a collection of `ITestCase` objects.

Each test is executed independently. If a test throws an `ArgumentException` due to invalid configuration, the runner records an `Error` result and continues with the remaining tests.

**3. Result Aggregation**

`TestSummary` stores individual execution results and provides aggregated statistics.

The overall suite status is:

- `Passed`: All tests pass.
- `Failed`: At least one test fails or produces an error.
- `NoTests`: No tests are executed.

**4. Reporting**

`JsonReportExporter` uses `System.Text.Json` to export test results into a human-readable JSON file.

## Example Output

```text
Voltage Test 1: Error
Reason: MinLimit cannot exceed maxlimit
Voltage Test 2: Failed
Voltage Test 3: Passed
Device Connection Test: Failed

Total tests: 4
Passed tests: 1
Failed tests: 2
Error tests: 1
Failed test names:
Voltage Test 2
Device Connection Test
Test suite status: Failed
```

The application also generates a `test-report.json` file containing individual test results and summary statistics.

## Getting Started

### Prerequisites

- .NET 10 SDK

### Run the Application

From the repository root:

```bash
dotnet run
```

### Run Unit Tests

```bash
dotnet test TestFlow.Tests/TestFlow.Tests.csproj
```

### Build the Project

```bash
dotnet build
```

## Testing

The project uses xUnit for automated unit testing.

The tests cover:

- Measurements within and outside allowed limits.
- Invalid measurement limits and exception handling.
- Execution of different test types.
- Exception isolation between test cases.
- Empty test suites.
- Overall test suite status.
- JSON file creation and serialized content.

## Design Decisions

**Interface-based design**

`ITestCase` allows the test runner to execute different test types without depending on their concrete implementations.

**Separation of responsibilities**

Test definitions, execution logic, result aggregation, and JSON reporting are implemented in separate classes.

**Exception isolation**

Individual test configuration errors are recorded without stopping the entire test execution process.

**Extensibility**

Additional test types can be introduced without modifying the core test runner, provided they implement the `ITestCase` interface.

## Project Scope

This project is a learning-oriented proof of concept for a test automation platform.

It currently uses predefined test cases and runs as a console application. It does not connect to physical measurement hardware.

Potential future improvements include configurable test cases, hardware integration, persistent result storage, and a REST API.