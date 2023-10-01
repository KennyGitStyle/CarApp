using System.Web;
using CarService.Helpers;

namespace CarService.Services;

public class CarsApiService : ICarsApiService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;
    private readonly string _apiHost;

    public CarsApiService(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _clientFactory = clientFactory;
        _apiKey = config["ApiSettings:ApiKey"] ?? string.Empty;
        _apiHost = config["ApiSettings:ApiHost"] ?? string.Empty;
    }

    public async Task<string> FetchCars(SearchFilterProps filters, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient("RapidApiClient");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _apiKey);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", _apiHost);
    
        var builder = new UriBuilder("https://cars-by-api-ninjas.p.rapidapi.com/v1/cars");
        var query = HttpUtility.ParseQueryString(builder.Query);
    
        if (!string.IsNullOrWhiteSpace(filters.Model))
        {
            query["model"] = filters.Model;
        }
    
        if (!string.IsNullOrWhiteSpace(filters.Manufacturer))
        {
            query["manufacturer"] = filters.Manufacturer;
        }
    
        if (!string.IsNullOrWhiteSpace(filters.FuelType))
        {
            query["fuelType"] = filters.FuelType;
        }
    
        if (filters.Year > 0)
        {
            query["year"] = filters.Year.ToString();
        }
    
        if (filters.Limit > 0)
        {
            query["limit"] = filters.Limit.ToString();
        }
    
        builder.Query = query.ToString();
        var url = builder.ToString();
    
        try
        {
            var response = await client.GetStringAsync(url, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return response;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
    }
}

