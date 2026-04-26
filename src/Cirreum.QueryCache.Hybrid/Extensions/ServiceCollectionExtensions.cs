namespace Cirreum.QueryCache.Hybrid.Extensions;

using Cirreum.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Extension methods for configuring hybrid query caching services.
/// </summary>
public static class ServiceCollectionExtensions {
	/// <summary>
	/// Registers the <see cref="HybridCacheableQueryService"/> as the implementation for
	/// <see cref="ICacheService"/>, enabling caching support for <c>ICacheableOperation&lt;T&gt;</c> requests.
	/// </summary>
	/// <remarks>
	/// This method requires <c>AddHybridCache()</c> to be called separately to configure the underlying
	/// <see cref="Microsoft.Extensions.Caching.Hybrid.HybridCache"/> service.
	/// </remarks>
	/// <param name="services">The service collection.</param>
	/// <returns>The service collection for chaining.</returns>
	public static IServiceCollection AddHybridQueryCaching(this IServiceCollection services) {
		//
		// Cache Service (HybridCache)
		//
		services
			.TryAddSingleton<ICacheService, HybridCacheableQueryService>();
		return services;
	}
}