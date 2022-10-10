using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class WizardViewModel : Conductor<IWizardStepViewModel>.Collection.OneActive
{
    private SpacecraftJourney? _journey;
    private Model.Model _model;

    public WizardViewModel(NameSelectionViewModel nameSelection, ShipSelectionViewModel shipSelection,
        PassengersSelectionViewModel passengersSelection,
        RouteSelectionViewModel routeSelection)
    {
        NameSelection = nameSelection;
        ShipSelection = shipSelection;
        PassengersSelection = passengersSelection;
        RouteSelection = routeSelection;
    }

    public Model.Model Model
    {
        get => _model;
        set => Set(ref _model, value);
    }

    public SpacecraftJourney? Journey
    {
        get => _journey;
        private set => Set(ref _journey, value);
    }

    public NameSelectionViewModel NameSelection { get; }

    public ShipSelectionViewModel ShipSelection { get; }

    public PassengersSelectionViewModel PassengersSelection { get; }

    public RouteSelectionViewModel RouteSelection { get; }

    public void ActivateStep(object step)
    {
        if (step is IWizardStepViewModel wizardStep)
            ActiveItem = wizardStep;
        else
            ActiveItem = ShipSelection;
    }

    protected override Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        NameSelection.Model = Model;
        ShipSelection.Model = Model;
        PassengersSelection.Model = Model;
        RouteSelection.Model = Model;

        return base.OnInitializeAsync(cancellationToken);
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Journey = new SpacecraftJourney(Model);

        NameSelection.Journey = Journey;
        ShipSelection.Journey = Journey;
        PassengersSelection.Journey = Journey;
        RouteSelection.Journey = Journey;

        return base.OnInitializeAsync(cancellationToken);
    }
}