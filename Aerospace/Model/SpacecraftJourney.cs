using System.Collections.Generic;

namespace Aerospace.Model;

internal struct SpacecraftJourney
{
    public SpacecraftJourney()
    {
        Spacecraft = default;
        NumPassengers = 0;
    }

    public Spacecraft Spacecraft { get; set; }

    public int NumPassengers { get; set; }

    public List<Planet> Route { get; } = new();
}