using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class MainViewModel : Conductor<Screen>
{
    private readonly IWindowManager _windowManager;
    private readonly WizardViewModel _wizardViewModel;

    public MainViewModel(WizardViewModel wizardViewModel, IWindowManager windowManager)
    {
        _wizardViewModel = wizardViewModel;
        _windowManager = windowManager;
    }

    public ObservableCollection<SpacecraftJourney> ActiveJourneys { get; } = new();

    public Model.Model Model { get; private set; }

    public async void CreateJourneyAsync()
    {
        var result = await _windowManager.ShowDialogAsync(_wizardViewModel);

        if (result == true &&
            _wizardViewModel.Journey is { } journey)
            ActiveJourneys.Add(journey);
    }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        var filename = Path.Combine("data", "data.json");
        await using var openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model.Model>(openStream, cancellationToken: cancellationToken);
        _wizardViewModel.Model = Model;
    }
}