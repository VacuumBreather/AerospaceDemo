using Aerospace.Model;

namespace Aerospace.ViewModels;

internal interface IWizardStepViewModel
{
    Model.Model Model { get; set; }

    SpacecraftJourney? Journey { get; set; }
}