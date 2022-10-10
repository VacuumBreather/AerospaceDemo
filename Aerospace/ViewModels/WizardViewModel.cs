using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
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

    protected override Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        foreach (var step in _wizardSteps) step.Model = Model;

        return base.OnInitializeAsync(cancellationToken);
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        var journey = new SpacecraftJourney(Model);

        foreach (var step in _wizardSteps) step.Journey = journey;

        ActiveItem = _wizardSteps.First();

        return base.OnActivateAsync(cancellationToken);
    }

    public void Cancel()
    {
        ActiveItem = _wizardSteps.First();
        var journey = new SpacecraftJourney(Model);

        foreach (var step in _wizardSteps) step.Journey = journey;
    }

    public void Next()
    {
        var currentIndex = _wizardSteps.IndexOf(ActiveItem);
        var nextIndex = (currentIndex + 1) % _wizardSteps.Count;
        var currentStep = _wizardSteps[nextIndex];

        ActiveItem = currentStep;
    }

    public void Back()
    {
        var currentIndex = _wizardSteps.IndexOf(ActiveItem);
        var nextIndex = (currentIndex + _wizardSteps.Count - 1) % _wizardSteps.Count;
        var currentStep = _wizardSteps[nextIndex];

        ActiveItem = currentStep;
    }
}