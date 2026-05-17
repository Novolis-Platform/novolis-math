#Requires -Version 7.0
param(
    [string]$Feed = $(if ($env:NOVOLIS_LOCAL_FEED) { $env:NOVOLIS_LOCAL_FEED } else { Join-Path (Split-Path $PSScriptRoot -Parent) "..\artifacts\nuget-local" })
)
$ErrorActionPreference = "Stop"
$Root = Split-Path $PSScriptRoot -Parent
New-Item -ItemType Directory -Force -Path $Feed | Out-Null
dotnet pack (Join-Path $Root "Novolis.Math.slnx") -c Release -o $Feed /p:ContinuousIntegrationBuild=false
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
Write-Host "Packed Novolis.Math -> $Feed"
