using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace Aerospace.Model;

internal class SpacecraftJourney : PropertyChangedBase
{
    private Spacecraft _spacecraft;
    private int _numPassengers;

    public SpacecraftJourney(Model model)
    {
        _spacecraft = model.Spacecrafts.First();
        Route.Add(model.Planets.First(planet => planet.Index == 3));
    }

    public Spacecraft Spacecraft
    {
        get => _spacecraft;
        set => Set(ref _spacecraft, value);
    }

    public int NumPassengers
    {
        get => _numPassengers;
        set => Set(ref _numPassengers, value);
    }

    public ObservableCollection<Planet> Route { get; } = new();
}