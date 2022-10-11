using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class WizardViewModel : Conductor<IWizardStepViewModel>.Collection.OneActive
{
    #region Constants and Fields

    private SpacecraftJourney? journey;
    private Model.Model model;

    #endregion

    #region Constructors and Destructors

    public WizardViewModel(NameSelectionViewModel nameSelection, ShipSelectionViewModel shipSelection,
        PassengersSelectionViewModel passengersSelection,
        RouteSelectionViewModel routeSelection)
    {
        NameSelection = nameSelection;
        ShipSelection = shipSelection;
        PassengersSelection = passengersSelection;
        RouteSelection = routeSelection;
    }

    #endregion

    #region Public Properties

    public SpacecraftJourney? Journey
    {
        get => journey;
        private set => Set(ref journey, value);
    }

    public Model.Model Model
    {
        get => model;
        set => Set(ref model, value);
    }

    public NameSelectionViewModel NameSelection { get; }

    public PassengersSelectionViewModel PassengersSelection { get; }

    public RouteSelectionViewModel RouteSelection { get; }

    public ShipSelectionViewModel ShipSelection { get; }

    #endregion

    #region Public Methods

    public void ActivateStep(object step)
    {
        if (step is IWizardStepViewModel wizardStep)
        {
            ActiveItem = wizardStep;
        }
        else
        {
            ActiveItem = ShipSelection;
        }
    }

    #endregion

    #region Protected Methods

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        Journey = new SpacecraftJourney(Model);

        NameSelection.Journey = Journey;
        ShipSelection.Journey = Journey;
        PassengersSelection.Journey = Journey;
        RouteSelection.Journey = Journey;

        return base.OnInitializeAsync(cancellationToken);
    }

    protected override Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        NameSelection.Model = Model;
        ShipSelection.Model = Model;
        PassengersSelection.Model = Model;
        RouteSelection.Model = Model;

        return base.OnInitializeAsync(cancellationToken);
    }

    #endregion
}