using System.Linq;
using System.Text.Json.Serialization;
using Caliburn.Micro;

namespace Aerospace.Model;

internal class SpacecraftJourney : PropertyChangedBase
{
    #region Constants and Fields

    private readonly Model model;

    private string name;
    private int numPassengers;
    private Spacecraft spacecraft;

    #endregion

    #region Constructors and Destructors

    public SpacecraftJourney(Model model)
    {
        this.model = model;
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

    [JsonIgnore]
    public double AdjustedMaxTravelDistance
    {
        get
        {
            double efficiency = (spacecraft.Capacity - NumPassengers) / (double) spacecraft.Capacity;
            double distanceFactor = 0.7 + (0.3 * efficiency);

            return spacecraft.MaxTravelDistance * distanceFactor;
        }
    }

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
        set
        {
            Set(ref numPassengers, value);
            NotifyOfPropertyChange(nameof(AdjustedMaxTravelDistance));
        }
    }

    [JsonIgnore]
    public double RemainingTravelDistance => AdjustedMaxTravelDistance - TotalTravelDistance;

    [JsonPropertyName("route")]
    public BindableCollection<Planet> Route { get; } = new();

    [JsonPropertyName("spacecraft")]
    public Spacecraft Spacecraft
    {
        get => spacecraft;
        set
        {
            Set(ref spacecraft, value);
            NotifyOfPropertyChange(nameof(AdjustedMaxTravelDistance));
        }
    }

    [JsonIgnore]
    public double TotalTravelDistance
    {
        get
        {
            var totalDistance = 0.0;

            if (Route.Count < 2)
            {
                return totalDistance;
            }

            Planet earth = Route.First(planet => planet.Index == 3);
            Planet previousPlanet = earth;

            foreach (Planet planet in Route)
            {
                totalDistance += Planet.CalculateDistanceBetween(previousPlanet, planet);
                previousPlanet = planet;
            }

            totalDistance += Planet.CalculateDistanceBetween(previousPlanet, earth);

            return totalDistance;
        }
    }

    #endregion
}