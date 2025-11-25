# Cirreum.QueryCache.Hybrid

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.QueryCache.Hybrid.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.QueryCache.Hybrid/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.QueryCache.Hybrid.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.QueryCache.Hybrid/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.QueryCache.Hybrid?style=flat-square&labelColor=1F1F1F&color=FF3B2E)](https://github.com/cirreum/Cirreum.QueryCache.Hybrid/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.QueryCache.Hybrid?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.QueryCache.Hybrid/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Hybrid caching implementation for Conductor Cacheable Queries**

## Overview

**Cirreum.QueryCache.Hybrid** provides a hybrid caching implementation that bridges Microsoft's `HybridCache` service with Cirreum's Conductor framework for cacheable queries.

This library implements the `ICacheableQueryService` interface using Microsoft's new `HybridCache` infrastructure, enabling automatic caching of query results with support for both local and distributed cache layers, tag-based invalidation, and failure-specific expiration policies.

## Features

- **Hybrid Caching**: Leverages Microsoft's `HybridCache` for optimal performance across local and distributed cache layers
- **Tag-Based Invalidation**: Support for cache invalidation using tags for related data
- **Failure Handling**: Configurable expiration times for failed operations to prevent cache stampedes
- **Result-Aware Caching**: Special handling for `IResult` responses with automatic failure detection
- **Simple Integration**: Single extension method for DI container registration

## Installation

```bash
dotnet add package Cirreum.QueryCache.Hybrid
```

## Usage

### Basic Setup

```csharp
using Cirreum.QueryCache.Hybrid.Extensions;

// Register services
services.AddHybridCache(); // Microsoft's HybridCache service
services.AddHybridQueryCaching(); // This library's implementation
```

### Query Implementation

```csharp
public record GetUserQuery(int UserId) : ICacheableQuery<User>
{
    public QueryCacheSettings CacheSettings => new()
    {
        Expiration = TimeSpan.FromMinutes(15),
        LocalExpiration = TimeSpan.FromMinutes(5),
        FailureExpiration = TimeSpan.FromMinutes(1)
    };
    
    public string[] CacheTags => [$"user:{UserId}"];
}
```

The `HybridCacheableQueryService` automatically handles:
- Cache key generation
- Cache-or-create patterns
- Failure result caching with shorter expiration
- Tag-based cache invalidation

## Contribution Guidelines

1. **Be conservative with new abstractions**  
   The API surface must remain stable and meaningful.

2. **Limit dependency expansion**  
   Only add foundational, version-stable dependencies.

3. **Favor additive, non-breaking changes**  
   Breaking changes ripple through the entire ecosystem.

4. **Include thorough unit tests**  
   All primitives and patterns should be independently testable.

5. **Document architectural decisions**  
   Context and reasoning should be clear for future maintainers.

6. **Follow .NET conventions**  
   Use established patterns from Microsoft.Extensions.* libraries.

## Versioning

Cirreum.QueryCache.Hybrid follows [Semantic Versioning](https://semver.org/):

- **Major** - Breaking API changes
- **Minor** - New features, backward compatible
- **Patch** - Bug fixes, backward compatible

Given its foundational role, major version bumps are rare and carefully considered.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**  
*Layered simplicity for modern .NET*