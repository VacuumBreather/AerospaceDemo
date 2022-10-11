using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using Caliburn.Micro;

namespace Aerospace.Model;

internal class SpacecraftJourney : PropertyChangedBase
{
    #region Constants and Fields

    private string name;
    private int numPassengers;
    private Spacecraft spacecraft;

    #endregion

    #region Constructors and Destructors

    public SpacecraftJourney(Model model)
    {
        name = "New Journey";
        spacecraft = model.Spacecrafts.First();
        Route.Add(model.Planets.First(planet => planet.Index == 3));
        Route.CollectionChanged += (_, __) =>
        {
            NotifyOfPropertyChange(nameof(TotalTravelDistance));
            NotifyOfPropertyChange(nameof(RemainingTravelDistance));
        };
    }

    #endregion

    #region Public Properties

    [JsonPropertyName("name")]
    public string Name
    {
        get => name;
        set => Set(ref name, value);
    }

    [JsonPropertyName("numPassengers")]
    public int NumPassengers
    {
        get => numPassengers;
        set => Set(ref numPassengers, value);
    }

    public double RemainingTravelDistance => 0;

    [JsonPropertyName("route")]
    public ObservableCollection<Planet> Route { get; } = new();

    [JsonPropertyName("spacecraft")]
    public Spacecraft Spacecraft
    {
        get => spacecraft;
        set => Set(ref spacecraft, value);
    }

    public double TotalTravelDistance => 0;

    #endregion
}