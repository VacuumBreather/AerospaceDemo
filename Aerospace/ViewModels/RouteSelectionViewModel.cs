using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;

namespace Aerospace.ViewModels;

internal class RouteSelectionViewModel : WizardStepViewModelBase
{
    #region Constants and Fields

    private Planet selectedPlanet;

    #endregion

    #region Public Properties

    public Planet SelectedPlanet
    {
        get => selectedPlanet;
        set
        {
            Set(ref selectedPlanet, value);

            if (SelectedPlanet.Index != Journey!.Route.Last().Index)
            {
                Journey!.Route.Add(selectedPlanet);
            }
        }
    }

    #endregion

    #region Protected Methods

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        SelectedPlanet = Journey!.Route.Last();

        return base.OnActivateAsync(cancellationToken);
    }

    #endregion
}