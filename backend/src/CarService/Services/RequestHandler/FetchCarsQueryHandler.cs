using CarService.Services;
using CarService.Services.Request;
using MediatR;

namespace CarService;

public class FetchCarsQueryHandler : IRequestHandler<FetchCarsQueryRequest, string>
{
    private readonly ICarsApiService _carsService;

    public FetchCarsQueryHandler(ICarsApiService carsService)
    {
        _carsService = carsService;
    }

    public async Task<string> Handle(FetchCarsQueryRequest request, CancellationToken cancellationToken)
    {
        return await _carsService.FetchCars(request.Filters, cancellationToken); // Pass the model from the request
    }
}
