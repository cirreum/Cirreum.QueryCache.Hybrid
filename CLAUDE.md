# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 10.0 library package (`Cirreum.QueryCache.Hybrid`) that provides a hybrid caching implementation for the Cirreum Conductor framework's cacheable queries. The library bridges Microsoft's `HybridCache` service with Cirreum's `ICacheableQueryService` interface.

## Common Development Commands

### Build Commands
```bash
# Restore dependencies
dotnet restore Cirreum.QueryCache.Hybrid.slnx

# Build the solution
dotnet build Cirreum.QueryCache.Hybrid.slnx --configuration Release --no-restore

# Pack for NuGet
dotnet pack Cirreum.QueryCache.Hybrid.slnx --configuration Release --no-build --output ./artifacts
```

### Local Development
```bash
# Build in Debug mode for local development
dotnet build Cirreum.QueryCache.Hybrid.slnx --configuration Debug

# Build with local version (uses version 1.0.100-rc for local Release builds)
dotnet build Cirreum.QueryCache.Hybrid.slnx --configuration Release
```

## Architecture

### Core Components

1. **HybridCacheableQueryService** (`src/Cirreum.QueryCache.Hybrid/HybridCacheableQueryService.cs:7`)
   - Main implementation of `ICacheableQueryService` using Microsoft's `HybridCache`
   - Handles cache-or-create patterns with failure expiration logic
   - Supports cache invalidation by key and tags

2. **ServiceCollectionExtensions** (`src/Cirreum.QueryCache.Hybrid/Extensions/ServiceCollectionExtensions.cs:21`)
   - Provides `AddHybridQueryCaching()` extension method for DI registration
   - Registers `HybridCacheableQueryService` as singleton implementation of `ICacheableQueryService`

### Key Dependencies
- `Cirreum.Core` (v1.0.16) - Provides the Conductor framework interfaces
- `Microsoft.Extensions.Caching.Hybrid` (v10.0.0) - Microsoft's hybrid caching implementation

### Caching Behavior
- Supports both success and failure response caching with different expiration times
- Implements tag-based cache invalidation
- Automatically handles local vs distributed cache expiration settings
- Special logic for `IResult` responses to cache failures with shorter expiration

## Project Structure

- **Solution**: Uses `.slnx` format (Visual Studio solution)
- **Build System**: MSBuild with custom `.props` files in `/build/` folder
- **Versioning**: Automatic versioning for CI/CD, local builds use 1.0.100-rc
- **Package Management**: Configured for NuGet publishing via GitHub Actions
- **Target Framework**: .NET 10.0 with latest C# language version and nullable reference types enabled

## CI/CD

The project uses GitHub Actions for automated publishing to NuGet. The workflow is triggered on releases and handles:
- Version extraction from git tags
- Building and packing the library
- Publishing to NuGet.org with OIDC authentication

## Usage Pattern

This library is designed to be used in conjunction with:
1. `Microsoft.Extensions.Caching.Hybrid` for the underlying cache service
2. `Cirreum.Conductor` framework for cacheable query patterns

Typical registration:
```csharp
services.AddHybridCache(); // Microsoft's service
services.AddHybridQueryCaching(); // This library's service
```