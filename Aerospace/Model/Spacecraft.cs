using System.Text.Json.Serialization;

namespace Aerospace.Model;

internal struct Spacecraft
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("capacity")] public int Capacity { get; set; }

    [JsonPropertyName("gravityGenerator")] public bool HasGravityGenerator { get; set; }

    [JsonPropertyName("maxTravelDistance")]
    public double MaxTravelDistance { get; set; }

    [JsonPropertyName("asteroidDeflector")]
    public bool MaxAsteroidDeflector { get; set; }

    public override string ToString()
    {
        return Name;
    }
}