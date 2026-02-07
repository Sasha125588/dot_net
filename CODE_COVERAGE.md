# Code Coverage Guide

This project uses xUnit v3 with Microsoft Testing Platform (MTP) for code coverage.

## Prerequisites

Install ReportGenerator globally (if not already installed):

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

## Running Coverage for All Tests

You have two options:

### Option 1: Using `dotnet test` (Recommended)

This is the simplest approach - it automatically discovers and runs all test projects:

```bash
./run-coverage-dotnet-test.sh
```

### Option 2: Using `dotnet run`

This explicitly runs each test project individually:

```bash
./run-all-coverage.sh
```

## Viewing the Report

After running either script, open the generated HTML report:

```bash
open CoverageReport/index.html
```

Or on Linux:

```bash
xdg-open CoverageReport/index.html
```

## How It Works

1. **Code Coverage Package**: All test projects include `Microsoft.Testing.Extensions.CodeCoverage` package
2. **MTP Configuration**: Test projects have `UseMicrosoftTestingPlatformRunner` and `TestingPlatformDotnetTestSupport` enabled
3. **Coverage Settings**: `Coverage.runsettings` excludes `Program` entry points from coverage reports
4. **Report Generation**: ReportGenerator combines all coverage XML files into a single HTML report

## Coverage Configuration

The `Coverage.runsettings` file excludes certain code from coverage:

```xml
<Functions>
  <Exclude>
    <Function>^Program\.</Function>
    <Function>.*\.Program\.</Function>
  </Exclude>
</Functions>
```

This ensures that entry point code (which is typically not unit-tested) doesn't affect coverage metrics.

## Project Structure

```
dot_net/
├── lab_1/
│   ├── 1.3/              # Project code
│   ├── 2/                # Project code
│   └── tests/
│       ├── 1.3Test/      # Tests for 1.3
│       └── 2Test/        # Tests for 2
├── utils/                # Shared utilities
├── Coverage.runsettings  # Coverage configuration
├── run-all-coverage.sh   # Coverage script (dotnet run)
├── run-coverage-dotnet-test.sh  # Coverage script (dotnet test)
└── CoverageReport/       # Generated HTML reports
```

## Adding New Test Projects

When adding a new test project, ensure it includes:

1. **Code Coverage Package**:
   ```xml
   <PackageReference Include="Microsoft.Testing.Extensions.CodeCoverage" Version="18.4.1" />
   ```

2. **MTP Configuration**:
   ```xml
   <PropertyGroup>
     <UseMicrosoftTestingPlatformRunner>true</UseMicrosoftTestingPlatformRunner>
     <TestingPlatformDotnetTestSupport>true</TestingPlatnetTestSupport>
   </PropertyGroup>
   ```

3. If using `run-all-coverage.sh`, add the project path to the `TEST_PROJECTS` array.

## Troubleshooting

### Coverage files not generated

Ensure your test project has:
- `Microsoft.Testing.Extensions.CodeCoverage` package reference
- `UseMicrosoftTestingPlatformRunner` set to `true`
- `TestingPlatformDotnetTestSupport` set to `true`

### ReportGenerator not found

Install it globally:

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

### Tests fail to run

Restore packages first:

```bash
dotnet restore
```

## CI/CD Integration

For CI/CD pipelines, use `dotnet test` with coverage arguments:

```bash
dotnet test dot_net.slnx -- \
  --coverage \
  --coverage-output-format cobertura \
  --coverage-output "coverage.cobertura.xml" \
  --coverage-settings Coverage.runsettings
```

Then collect all `coverage.cobertura.xml` files from `TestResults` directories for reporting.
