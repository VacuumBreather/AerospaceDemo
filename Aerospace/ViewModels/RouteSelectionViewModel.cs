using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class RouteSelectionViewModel : WizardStepViewModelBase
{
    #region Constants and Fields

    private Planet selectedPlanet;

    #endregion

    #region Public Properties

    public bool CanRemoveLastPlanet => Journey!.Route.Count > 1;

    public BindableCollection<Planet> FilteredPlanets { get; } = new();

    public Planet SelectedPlanet
    {
        get => selectedPlanet;
        set
        {
            Set(ref selectedPlanet, value);

            if (SelectedPlanet.Index != Journey!.Route.Last().Index)
            {
                Journey!.Route.Add(selectedPlanet);
                UpdateFilteredPlanets();
            }

            NotifyOfPropertyChange(nameof(CanRemoveLastPlanet));
        }
    }

    #endregion

    #region Public Methods

    public void OptimizeRoute()
    {
        var currentRoute = Journey!.Route.ToList();

        if (currentRoute.Last().Index != 3)
        {
            currentRoute.Add(Model.Planets.First(planet => planet.Index == 3));
        }

        var groupedRoute = currentRoute.GroupBy(planet => planet.Index).ToArray();
        int neededLists = groupedRoute.Max(group => group.Count());

        var routeSegments = new List<Planet>[neededLists];

        for (var i = 0; i < routeSegments.Length; i++)
        {
            routeSegments[i] = new List<Planet>();
        }

        foreach (var grouping in groupedRoute)
        {
            for (var i = 0; i < grouping.Count(); i++)
            {
                routeSegments[i].Add(grouping.First());
            }
        }

        var optimizedRoute = new List<Planet>();

        for (var i = 0; i < routeSegments.Length; i++)
        {
            optimizedRoute.AddRange(i % 2 == 0
                ? routeSegments[i]
                    .OrderBy(planet => planet.Index != 3)
                    .ThenBy(planet => planet.Index < 3)
                    .ThenBy(planet => planet.Index)
                    .DistinctBy(planet => planet.Index)
                : routeSegments[i]
                    .OrderByDescending(planet => planet.Index != 3)
                    .ThenByDescending(planet => planet.Index < 3)
                    .ThenByDescending(planet => planet.Index)
                    .DistinctBy(planet => planet.Index));
        }

        while (optimizedRoute.Last().Index == 3)
        {
            optimizedRoute.RemoveAt(optimizedRoute.Count - 1);
        }

        Journey!.Route.Clear();
        Journey!.Route.AddRange(optimizedRoute);
    }

    public void RemoveLastPlanet()
    {
        Journey!.Route.RemoveAt(Journey.Route.Count - 1);

        SelectedPlanet = Journey.Route.Last();
        UpdateFilteredPlanets();

        NotifyOfPropertyChange(nameof(CanRemoveLastPlanet));
    }

    #endregion

    #region Protected Methods

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        while (Journey!.TotalTravelDistance > Journey!.AdjustedMaxTravelDistance)
        {
            Journey!.Route.RemoveAt(Journey!.Route.Count - 1);
        }

        SelectedPlanet = Journey!.Route.Last();

        FilteredPlanets.Clear();
        FilteredPlanets.AddRange(Model.Planets);
        UpdateFilteredPlanets();

        return base.OnActivateAsync(cancellationToken);
    }

    #endregion

    #region Private Methods

    private void UpdateFilteredPlanets()
    {
        Planet earth = Model.Planets.First(planet => planet.Index == 3);

        FilteredPlanets.Clear();
        FilteredPlanets.AddRange(Model.Planets.Where(planet =>
            (planet.Index == Journey!.Route.Last().Index) ||
            ((Journey!.TotalTravelDistance - Planet.CalculateDistanceBetween(Journey!.Route.Last(), earth))
             + Planet.CalculateDistanceBetween(Journey!.Route.Last(), planet) +
             Planet.CalculateDistanceBetween(planet, earth)
             < Journey!.AdjustedMaxTravelDistance)));
    }

    #endregion
}