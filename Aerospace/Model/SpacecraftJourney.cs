using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;

namespace Aerospace.Model;

internal class SpacecraftJourney : PropertyChangedBase
{
    private Spacecraft _spacecraft;
    private int _numPassengers;
    private string _name;

    public SpacecraftJourney(Model model)
    {
        _name = "New Journey";
        _spacecraft = model.Spacecrafts.First();
        Route.Add(model.Planets.First(planet => planet.Index == 3));
    }

    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
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