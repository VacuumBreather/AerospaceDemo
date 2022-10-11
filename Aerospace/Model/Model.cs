using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aerospace.Model;

internal struct Model
{
    [JsonPropertyName("planets")]
    public List<Planet> Planets { get; set; }

    [JsonPropertyName("spacecrafts")]
    public List<Spacecraft> Spacecrafts { get; set; }
}