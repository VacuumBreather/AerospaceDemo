using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;

namespace Aerospace.ViewModels;

internal class RouteSelectionViewModel : WizardStepViewModelBase
{
    private Planet _selectedPlanet;

    public Planet SelectedPlanet
    {
        get => _selectedPlanet;
        set
        {
            Set(ref _selectedPlanet, value);

            if (SelectedPlanet.Index != Journey!.Route.Last().Index) Journey!.Route.Add(_selectedPlanet);
        }
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        SelectedPlanet = Journey!.Route.Last();

        return base.OnActivateAsync(cancellationToken);
    }
}