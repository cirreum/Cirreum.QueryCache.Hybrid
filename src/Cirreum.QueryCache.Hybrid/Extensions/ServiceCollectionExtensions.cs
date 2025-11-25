namespace Cirreum.QueryCache.Hybrid.Extensions;

using Cirreum.Conductor.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Extension methods for configuring hybrid query caching services.
/// </summary>
public static class ServiceCollectionExtensions {
	/// <summary>
	/// Registers the <see cref="HybridCacheableQueryService"/> as the implementation for
	/// <see cref="ICacheableQueryService"/>, enabling caching support for <c>ICacheableQuery&lt;T&gt;</c> requests.
	/// </summary>
	/// <remarks>
	/// This method requires <c>AddHybridCache()</c> to be called separately to configure the underlying
	/// <see cref="Microsoft.Extensions.Caching.Hybrid.HybridCache"/> service.
	/// </remarks>
	/// <param name="services">The service collection.</param>
	/// <returns>The service collection for chaining.</returns>
	public static IServiceCollection AddHybridQueryCaching(this IServiceCollection services) {
		//
		// Conductor Cacheable Query Service (HybridCache)
		//
		services
			.TryAddSingleton<ICacheableQueryService, HybridCacheableQueryService>();
		return services;
	}
}