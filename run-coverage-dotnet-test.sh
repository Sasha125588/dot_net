#!/usr/bin/env bash
# Run all tests using dotnet test with code coverage and generate HTML report.
# Requires: reportgenerator (dotnet tool install -g dotnet-reportgenerator-globaltool)
#
# This approach uses 'dotnet test' which automatically discovers and runs all test projects.

set -e
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPORT_DIR="$SCRIPT_DIR/CoverageReport"
COVERAGE_SETTINGS="$SCRIPT_DIR/Coverage.runsettings"

# Clean previous coverage results
rm -rf "$REPORT_DIR"
mkdir -p "$REPORT_DIR"

echo "=== Running all tests with code coverage using dotnet test ==="
echo ""

cd "$SCRIPT_DIR"

# Run all tests in the solution with coverage
dotnet test --solution dot_net.slnx -- \
  --coverage \
  --coverage-output-format cobertura \
  --coverage-output "coverage.cobertura.xml" \
  --coverage-settings "$COVERAGE_SETTINGS"

echo ""
echo "=== Collecting coverage files ==="

# Find all generated coverage files
COVERAGE_FILES=$(find . -path "*/TestResults/coverage.cobertura.xml" -type f)

if [ -z "$COVERAGE_FILES" ]; then
  echo "Error: No coverage files were generated"
  exit 1
fi

echo "Found coverage files:"
echo "$COVERAGE_FILES" | while read -r file; do
  echo "  - $file"
done

# Generate combined HTML report
echo ""
echo "=== Generating combined HTML report ==="
REPORTS_ARG=$(echo "$COVERAGE_FILES" | tr '\n' ';' | sed 's/;$//')
reportgenerator \
  -reports:"$REPORTS_ARG" \
  -targetdir:"$REPORT_DIR" \
  -reporttypes:Html

echo ""
echo "✓ Done! Open $REPORT_DIR/index.html to view the report"
