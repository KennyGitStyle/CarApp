using CarService.Helpers;
using MediatR;

namespace CarService.Services.Request;

public class FetchCarsQueryRequest : IRequest<string>
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiHost { get; set; } = string.Empty;
    public SearchFilterProps Filters { get; set; } = new();
    public CancellationToken CancellationToken { get; set; }
}
