param([string]$Configuration = 'Release')
$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($env:TestSqlServer)) {
    throw 'Set TestSqlServer to a SQL Server connection with permission to create and delete isolated CoachingTests_* databases.'
}
Push-Location (Split-Path -Parent $PSScriptRoot)
try {
    dotnet restore Coaching.slnx
    if ($LASTEXITCODE -ne 0) { throw 'Restore or vulnerability audit failed.' }
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) { throw 'EF tool restore failed.' }
    dotnet build Coaching.slnx --configuration $Configuration --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
    dotnet test Coaching.slnx --configuration $Configuration --no-build --logger 'trx;LogFileName=coaching-tests.trx' --results-directory artifacts/test-results
    if ($LASTEXITCODE -ne 0) { throw 'Tests failed.' }
    dotnet list Coaching.slnx package --vulnerable --include-transitive --format json --output-version 1 | Set-Content -Encoding utf8 artifacts/vulnerability-report.json
    if ($LASTEXITCODE -ne 0) { throw 'Vulnerability scan failed.' }
    $report = Get-Content -LiteralPath artifacts/vulnerability-report.json -Raw | ConvertFrom-Json
    if ($null -ne $report.problems -and @($report.problems).Count -gt 0) { throw 'Vulnerability reporting returned errors.' }
    foreach ($project in $report.projects) {
        foreach ($framework in $project.frameworks) {
            foreach ($package in @($framework.topLevelPackages) + @($framework.transitivePackages)) {
                if ($null -ne $package -and $null -ne $package.vulnerabilities -and @($package.vulnerabilities).Count -gt 0) { throw "Vulnerable package: $($package.id)" }
            }
        }
    }
    dotnet ef migrations has-pending-model-changes --project src/Coaching.Infrastructure --startup-project src/Coaching.Api --configuration $Configuration --no-build
    if ($LASTEXITCODE -ne 0) { throw 'The database model has changes without a migration.' }
    dotnet ef migrations script --idempotent --project src/Coaching.Infrastructure --startup-project src/Coaching.Api --configuration $Configuration --no-build --output artifacts/migrations.sql
    if ($LASTEXITCODE -ne 0) { throw 'Migration script generation failed.' }
    dotnet publish src/Coaching.Api --configuration $Configuration --no-build --output artifacts/api
    if ($LASTEXITCODE -ne 0) { throw 'Publishing failed.' }
}
finally { Pop-Location }
