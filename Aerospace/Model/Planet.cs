using System;
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

    public static double CalculateDistanceBetween(Planet firstPlanet, Planet secondPlanet)
    {
        if ((firstPlanet.Index == 3) || (secondPlanet.Index == 3) ||
            ((firstPlanet.Index > 3) && (secondPlanet.Index > 3)) ||
            ((firstPlanet.Index < 3) && (secondPlanet.Index < 3)))
        {
            // Either include earth or on same side of earth in regards to the sun.
            return Math.Abs(firstPlanet.DistanceFromEarth - secondPlanet.DistanceFromEarth);
        }

        // Either side of Earth in regards to the sun.
        return firstPlanet.DistanceFromEarth + secondPlanet.DistanceFromEarth;
    }

    public override string ToString() => Name;
}