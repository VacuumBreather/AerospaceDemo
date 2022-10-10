using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal abstract class WizardStepViewModelBase : Screen, IWizardStepViewModel
{
    private SpacecraftJourney? _journey;
    private Model.Model _model;

    public Model.Model Model
    {
        get => _model;
        set => Set(ref _model, value);
    }

    public SpacecraftJourney? Journey
    {
        get => _journey;
        set => Set(ref _journey, value);
    }
}