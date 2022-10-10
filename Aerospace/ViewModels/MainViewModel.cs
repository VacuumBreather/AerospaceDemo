using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
    private SpacecraftJourney? _selectedJourney;

    public MainViewModel(WizardViewModel wizardViewModel, IWindowManager windowManager)
    {
        _wizardViewModel = wizardViewModel;
        _windowManager = windowManager;
    }

    public ObservableCollection<SpacecraftJourney> ActiveJourneys { get; } = new();

    public Model.Model Model { get; private set; }

    public SpacecraftJourney? SelectedJourney
    {
        get => _selectedJourney;
        set
        {
            Set(ref _selectedJourney, value);
            NotifyOfPropertyChange(nameof(CanSaveJourneyAsync));
        }
    }

    public bool CanSaveJourneyAsync => SelectedJourney is not null;

    public async void CreateJourneyAsync()
    {
        var result = await _windowManager.ShowDialogAsync(_wizardViewModel);

        if (result == true &&
            _wizardViewModel.Journey is { } journey)
            ActiveJourneys.Add(journey);
    }

    public async void LoadJourneyAsync()
    {
        await Task.CompletedTask;
    }

    public async void SaveJourneyAsync()
    {
        await Task.CompletedTask;
    }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        var filename = Path.Combine("data", "data.json");
        await using var openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model.Model>(openStream, cancellationToken: cancellationToken);
        _wizardViewModel.Model = Model;
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (SelectedJourney is null) SelectedJourney = ActiveJourneys.FirstOrDefault();

        return base.OnActivateAsync(cancellationToken);
    }
}