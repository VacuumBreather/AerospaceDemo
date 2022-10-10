using System.Threading;
using System.Threading.Tasks;

namespace Aerospace.ViewModels;

internal class PassengersSelectionViewModel : WizardStepViewModelBase
{
    public PassengersSelectionViewModel() : base(1)
    {
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (Journey!.NumPassengers > Journey!.Spacecraft.Capacity)
            Journey!.NumPassengers = Journey!.Spacecraft.Capacity;

        return base.OnActivateAsync(cancellationToken);
    }
}