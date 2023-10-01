namespace CarService.Helpers;

public record SearchFilterProps
{
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Limit { get; set; }
}