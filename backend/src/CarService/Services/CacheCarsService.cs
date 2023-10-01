using CarService.Helpers;
using CarService.Services.Request;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CarService.Services;

public class CachingCarsService : ICarsApiService
{
    private const string CacheKey = "CarsData";
    private readonly IMemoryCache _cache;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public CachingCarsService(IMemoryCache cache, IMediator mediator, IConfiguration config)
    {
        _cache = cache;
        _mediator = mediator;
        _config = config;
    }
    public async Task<string> FetchCars(SearchFilterProps filter, CancellationToken cancellationToken = default)
    {
       if (!_cache.TryGetValue(CacheKey, out string? cachedData))
        {
            try
            {
                // Use MediatR to send a request to fetch data
                var request = new FetchCarsQueryRequest
                {
                    ApiKey = _config["ApiSettings:ApiKey"] ?? string.Empty,
                    ApiHost = _config["ApiSettings:ApiHost"] ?? string.Empty,
                    Filters = filter,
                    CancellationToken = cancellationToken
                };

                cachedData = await _mediator.Send(request, cancellationToken);
                
                // Cache the data
                _cache.Set(CacheKey, cachedData, TimeSpan.FromMinutes(10));
            }
            catch (OperationCanceledException ex)
            {
                // Handle cancellation if needed
                // You can choose to rethrow the exception or return an empty string
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
        return cachedData ?? string.Empty;
    }
}

