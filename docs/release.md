# Release

Packages publish as `Novolis.Math.*` on GitHub Packages under the `2026.1.*` line.

## Policy

- [Release policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/release-policy.md)
- [Package policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/package-policy.md) — README packed per package, XML docs required

## Local validation

```bash
dotnet build Novolis.Math.slnx
dotnet test Novolis.Math.slnx
pwsh -File ../novolis-governance/scripts/verify-nuget-only.ps1
```

## Versioning

Bump package versions in the repo `Directory.Packages.props` / project files per governance; consumers pin `2026.1.*` from GPR.
