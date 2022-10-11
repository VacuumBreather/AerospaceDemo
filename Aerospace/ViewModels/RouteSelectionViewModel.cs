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

    public bool CanRemoveLastPlanet => Journey!.Route.Count > 1;

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

            NotifyOfPropertyChange(nameof(CanRemoveLastPlanet));
        }
    }

    #endregion

    #region Public Methods

    public void RemoveLastPlanet()
    {
        Journey!.Route.RemoveAt(Journey.Route.Count - 1);

        SelectedPlanet = Journey.Route.Last();

        NotifyOfPropertyChange(nameof(CanRemoveLastPlanet));
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