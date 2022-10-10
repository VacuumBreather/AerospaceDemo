using System.Text.Json.Serialization;

namespace Aerospace.Model;

internal struct Planet
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("positionIndex")]
    public int Index { get; set; }

    [JsonPropertyName("habitable")]
    public bool IsHabitable { get; set; }

    [JsonPropertyName("diameter")]
    public double Diameter { get; set; }

    [JsonPropertyName("averageTemperature")]
    public double AverageTemperature { get; set; }

    [JsonPropertyName("distanceFromEarth")]
    public double DistanceFromEarth { get; set; }

    [JsonPropertyName("isDwarf")]
    public bool IsDwarf { get; set; }
}