using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class WizardViewModel : Conductor<IWizardStepViewModel>
{
    private readonly IList<IWizardStepViewModel> _wizardSteps;

    public WizardViewModel(IEnumerable<IWizardStepViewModel> wizardSteps)
    {
        _wizardSteps = wizardSteps.OrderBy(step => step.Index).ToList();
    }

    public Model.Model Model { get; set; }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var currentStep = _wizardSteps.First();

        currentStep.Model = Model;

        ActiveItem = currentStep;

        return base.OnActivateAsync(cancellationToken);
    }

    public void Cancel()
    {
        var currentStep = _wizardSteps.First();

        currentStep.Model = Model;

        ActiveItem = currentStep;
    }

    public void Next()
    {
        var currentIndex = _wizardSteps.IndexOf(ActiveItem);
        var nextIndex = (currentIndex + 1) % _wizardSteps.Count;
        var currentStep = _wizardSteps[nextIndex];
        currentStep.Model = Model;

        ActiveItem = currentStep;
    }

    public void Back()
    {
        var currentIndex = _wizardSteps.IndexOf(ActiveItem);
        var nextIndex = (currentIndex + _wizardSteps.Count - 1) % _wizardSteps.Count;
        var currentStep = _wizardSteps[nextIndex];
        currentStep.Model = Model;

        ActiveItem = currentStep;
    }
}