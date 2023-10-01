using CarService;
using CarService.Helpers;
using CarService.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
// Add configuration settings
builder.Configuration.AddJsonFile("appsettings.Development.json");
builder.Services.AddHttpClient("RapidApiClient");  // Registering the HttpClient with a name
builder.Services.AddMediatR(typeof(FetchCarsQueryHandler));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICarsApiService, CarsApiService>();
builder.Services.AddSingleton<CachingCarsService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // Replace with the actual URL of your Next.js frontend
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/api/cars", async (HttpContext context, ICarsApiService carsApi) =>
{
    var filters = new SearchFilterProps
    {
        Manufacturer = context.Request.Query["manufacturer"].ToString(),
        Model = context.Request.Query["model"].ToString(),
        FuelType = context.Request.Query["fuelType"].ToString(),
        Year = int.TryParse(context.Request.Query["year"], out int year) ? year : 2022,
        Limit = int.TryParse(context.Request.Query["limit"], out int limit) ? limit : 10
    };


    // Use the model parameter when fetching cars
    var carsData = await carsApi.FetchCars(filters, CancellationToken.None);
    await context.Response.WriteAsync(carsData);
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status500InternalServerError)
.WithName("Get cars")
.WithDescription("Get Cars");


app.Run();