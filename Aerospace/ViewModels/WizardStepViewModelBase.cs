using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal abstract class WizardStepViewModelBase : Screen, IWizardStepViewModel
{
    protected WizardStepViewModelBase(int index)
    {
        Index = index;
    }

    public int Index { get; init; }

    public Model.Model Model { get; set; }
}