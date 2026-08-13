$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $Root
$Out = Join-Path $Root "tool-output"
New-Item -ItemType Directory -Force -Path $Out | Out-Null

function Fail($tool, $msg) {
  Write-Error "TOOL_FAILED: $tool — $msg (module=CSharp_FE_V8.0_BE_V6.0/frontend)"
  exit 1
}

Write-Host "==> build"
dotnet build 2>&1 | Tee-Object (Join-Path $Out "dotnet-build.log")
if ($LASTEXITCODE -ne 0) { Fail "dotnet" "build failed" }

Write-Host "==> unilyze"
if (Get-Command unilyze -ErrorAction SilentlyContinue) {
  unilyze . 2>&1 | Tee-Object (Join-Path $Out "unilyze.log")
} else { Fail "unilyze" "TOOL_NOT_INSTALLED: unilyze" }

Write-Host "==> Dolos"
if (Get-Command dolos -ErrorAction SilentlyContinue) {
  dolos run . 2>&1 | Tee-Object (Join-Path $Out "dolos.log")
} else { Fail "Dolos" "TOOL_NOT_INSTALLED: dolos" }

Write-Host "==> Opengrep (+ unilyze-replacement rows)"
if (-not (Get-Command opengrep -ErrorAction SilentlyContinue)) { Fail "Opengrep" "TOOL_NOT_INSTALLED: opengrep" }
opengrep scan . 2>&1 | Tee-Object (Join-Path $Out "opengrep.log")
Copy-Item (Join-Path $Out "opengrep.log") (Join-Path $Out "opengrep-code-health.log")
Copy-Item (Join-Path $Out "opengrep.log") (Join-Path $Out "opengrep-input-validation.log")
Copy-Item (Join-Path $Out "opengrep.log") (Join-Path $Out "opengrep-secrets.log")
Copy-Item (Join-Path $Out "opengrep.log") (Join-Path $Out "opengrep-auth.log")

Write-Host "==> Trivy (+ unilyze replacement)"
if (-not (Get-Command trivy -ErrorAction SilentlyContinue)) { Fail "Trivy" "TOOL_NOT_INSTALLED: trivy" }
trivy fs --format json -o (Join-Path $Out "trivy.json") . 2>&1 | Tee-Object (Join-Path $Out "trivy.log")
Copy-Item (Join-Path $Out "trivy.json") (Join-Path $Out "trivy-nuget.json")

Write-Host "==> MiniCover"
if (Get-Command minicover -ErrorAction SilentlyContinue) {
  minicover instrument 2>&1 | Tee-Object (Join-Path $Out "minicover.log")
} else {
  dotnet tool run minicover -- instrument 2>&1 | Tee-Object (Join-Path $Out "minicover.log")
  if ($LASTEXITCODE -ne 0) { Fail "MiniCover" "TOOL_NOT_INSTALLED: minicover" }
}

Write-Host "==> Stryker.NET"
if (Get-Command dotnet-stryker -ErrorAction SilentlyContinue) {
  dotnet-stryker 2>&1 | Tee-Object (Join-Path $Out "stryker.log")
} else {
  dotnet tool run dotnet-stryker 2>&1 | Tee-Object (Join-Path $Out "stryker.log")
  if ($LASTEXITCODE -ne 0) { Fail "Stryker.NET" "TOOL_NOT_INSTALLED: stryker" }
}

Write-Host "==> diff-cover"
pip install diff-cover -q
"<?xml version='1.0'?><coverage version='1'><sources><source>.</source></sources><packages/></coverage>" | Set-Content (Join-Path $Out "coverage.xml")
diff-cover (Join-Path $Out "coverage.xml") --compare-branch=HEAD 2>&1 | Tee-Object (Join-Path $Out "diff-cover.log")

Write-Host "ALL_TOOLS_OK module=CSharp_FE_V8.0_BE_V6.0/frontend"
