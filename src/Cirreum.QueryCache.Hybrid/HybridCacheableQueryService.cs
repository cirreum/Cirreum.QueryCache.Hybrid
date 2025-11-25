namespace Cirreum.QueryCache.Hybrid;

using Cirreum.Conductor.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using System.Collections.Generic;

sealed class HybridCacheableQueryService(
	HybridCache hybridCache
) : ICacheableQueryService {

	public async ValueTask<TResponse> GetOrCreateAsync<TResponse>(
		string cacheKey,
		Func<CancellationToken, ValueTask<TResponse>> factory,
		QueryCacheSettings settings,
		string[]? tags = null,
		CancellationToken cancellationToken = default) {

		var factoryExecuted = false;

		var value = await hybridCache.GetOrCreateAsync(
			cacheKey,
			async ct => {
				factoryExecuted = true;
				return await factory(ct);
			},
			options: CreateOptions(settings),
			tags: tags,
			cancellationToken: cancellationToken);

		// Only update expiration if we just executed the factory and got a failure
		if (factoryExecuted &&
			value is IResult { IsSuccess: false } &&
			settings.FailureExpiration.HasValue) {
			await hybridCache.SetAsync(
				cacheKey,
				value,
				CreateOptions(settings, useFailureExpiration: true),
				tags: tags,
				cancellationToken: cancellationToken);
		}

		return value;
	}

	public ValueTask RemoveAsync(string cacheKey, CancellationToken cancellationToken) {
		return hybridCache.RemoveAsync(cacheKey, cancellationToken);
	}

	public ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken = default) {
		return hybridCache.RemoveByTagAsync(tag, cancellationToken);
	}

	public ValueTask RemoveByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default) {
		return hybridCache.RemoveByTagAsync(tags, cancellationToken);
	}

	private static HybridCacheEntryOptions CreateOptions(QueryCacheSettings settings, bool useFailureExpiration = false) {
		var expiration = useFailureExpiration && settings.FailureExpiration.HasValue
			? settings.FailureExpiration.Value
			: settings.Expiration;

		var localExpiration = useFailureExpiration && settings.FailureExpiration.HasValue
			? settings.FailureExpiration.Value
			: settings.LocalExpiration;

		return new HybridCacheEntryOptions {
			Expiration = expiration,
			LocalCacheExpiration = localExpiration
		};
	}

}