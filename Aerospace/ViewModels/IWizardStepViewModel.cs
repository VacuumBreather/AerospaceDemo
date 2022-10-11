using Aerospace.Model;

namespace Aerospace.ViewModels;

internal interface IWizardStepViewModel
{
    SpacecraftJourney? Journey { get; set; }
    Model.Model Model { get; set; }
}