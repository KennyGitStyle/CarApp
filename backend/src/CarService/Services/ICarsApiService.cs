using CarService.Helpers;

namespace CarService.Services;

public interface ICarsApiService
{
    Task<string> FetchCars(SearchFilterProps searchFilter, CancellationToken cancellationToken = default);
}

