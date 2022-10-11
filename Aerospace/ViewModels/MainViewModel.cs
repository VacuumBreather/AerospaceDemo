using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Aerospace.Model;
using Caliburn.Micro;
using Microsoft.Win32;

namespace Aerospace.ViewModels;

internal class MainViewModel : Conductor<Screen>
{
    private readonly IWindowManager _windowManager;
    private readonly WizardViewModel _wizardViewModel;
    private SpacecraftJourney? _selectedJourney;
    private JsonSerializerOptions? _serializationOptions;

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
        var openFileDialog = new OpenFileDialog
        {
            Title = "Save route",
            DefaultExt = "json",
            Filter = "json files (*.json)|*.json",
            CheckFileExists = true,
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        var result = openFileDialog.ShowDialog();

        if (result == true)
        {
            var filename = openFileDialog.FileName;

            await using var readStream = File.OpenRead(filename);

            var journey = await JsonSerializer.DeserializeAsync<SpacecraftJourney>(readStream, _serializationOptions);

            if (journey != null)
                ActiveJourneys.Add(journey);
        }
    }

    public async void SaveJourneyAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save route",
            DefaultExt = "json",
            Filter = "json files (*.json)|*.json",
            CheckPathExists = true,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        var result = saveFileDialog.ShowDialog();

        if (result == true)
        {
            var filename = saveFileDialog.FileName;

            await using var writeStream = File.OpenWrite(filename);
            await JsonSerializer.SerializeAsync(writeStream, SelectedJourney!, _serializationOptions);
        }
    }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        var filename = Path.Combine("data", "data.json");
        await using var openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model.Model>(openStream, cancellationToken: cancellationToken);
        _wizardViewModel.Model = Model;

        _serializationOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JourneyConverter(Model)
            }
        };
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        if (SelectedJourney is null) SelectedJourney = ActiveJourneys.FirstOrDefault();

        return base.OnActivateAsync(cancellationToken);
    }
}