using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using Caliburn.Micro;

namespace Aerospace.Model;

internal class SpacecraftJourney : PropertyChangedBase
{
    private string _name;
    private int _numPassengers;
    private Spacecraft _spacecraft;

    public SpacecraftJourney(Model model)
    {
        _name = "New Journey";
        _spacecraft = model.Spacecrafts.First();
        Route.Add(model.Planets.First(planet => planet.Index == 3));
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    [JsonPropertyName("spacecraft")]
    public Spacecraft Spacecraft
    {
        get => _spacecraft;
        set => Set(ref _spacecraft, value);
    }

    [JsonPropertyName("numPassengers")]
    public int NumPassengers
    {
        get => _numPassengers;
        set => Set(ref _numPassengers, value);
    }

    [JsonPropertyName("route")] public ObservableCollection<Planet> Route { get; } = new();
}