namespace Aerospace.ViewModels;

internal interface IWizardStepViewModel
{
    int Index { get; }

    Model.Model Model { get; set; }
}