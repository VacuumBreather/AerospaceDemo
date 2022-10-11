using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal abstract class WizardStepViewModelBase : Screen, IWizardStepViewModel
{
    #region Constants and Fields

    private SpacecraftJourney? journey;
    private Model.Model model;

    #endregion

    #region IWizardStepViewModel Implementation

    public SpacecraftJourney? Journey
    {
        get => journey;
        set => Set(ref journey, value);
    }

    public Model.Model Model
    {
        get => model;
        set => Set(ref model, value);
    }

    #endregion
}