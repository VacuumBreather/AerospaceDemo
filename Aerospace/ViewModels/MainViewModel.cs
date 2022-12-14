using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Aerospace.Model;
using Caliburn.Micro;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace Aerospace.ViewModels;

internal class MainViewModel : Conductor<Screen>
{
    #region Constants and Fields

    private readonly IWindowManager windowManager;
    private readonly WizardViewModel wizardViewModel;
    private SpacecraftJourney? selectedJourney;
    private JsonSerializerOptions? serializationOptions;
    private Model.Model model;

    #endregion

    #region Constructors and Destructors

    public MainViewModel(WizardViewModel wizardViewModel, IWindowManager windowManager)
    {
        this.wizardViewModel = wizardViewModel;
        this.windowManager = windowManager;
    }

    #endregion

    #region Public Properties

    public ObservableCollection<SpacecraftJourney> ActiveJourneys { get; } = new();

    public bool CanDeleteJourney => SelectedJourney is not null;

    public bool CanSaveJourneyAsync => SelectedJourney is not null;

    public Model.Model Model
    {
        get => model;
        private set => Set(ref model, value);
    }

    public SpacecraftJourney? SelectedJourney
    {
        get => selectedJourney;
        set
        {
            Set(ref selectedJourney, value);
            NotifyOfPropertyChange(nameof(CanSaveJourneyAsync));
            NotifyOfPropertyChange(nameof(CanDeleteJourney));
        }
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async void CreateJourneyAsync()
    {
        IDictionary<string, object> settings = new Dictionary<string, object>();
        settings["WindowStartupLocation"] = WindowStartupLocation.CenterScreen;
        settings["ResizeMode"] = ResizeMode.NoResize;

        bool? result = await windowManager.ShowDialogAsync(wizardViewModel, settings: settings);

        if ((result == true) &&
            wizardViewModel.Journey is { } journey)
        {
            ActiveJourneys.Add(journey);
        }
    }

    [UsedImplicitly]
    public void DeleteJourney() => ActiveJourneys.Remove(SelectedJourney!);

    [UsedImplicitly]
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

        bool? result = openFileDialog.ShowDialog();

        if (result == true)
        {
            string filename = openFileDialog.FileName;

            await using FileStream readStream = File.OpenRead(filename);

            var journey = await JsonSerializer.DeserializeAsync<SpacecraftJourney>(readStream, serializationOptions);

            if (journey != null)
            {
                ActiveJourneys.Add(journey);
            }
        }
    }

    [UsedImplicitly]
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

        bool? result = saveFileDialog.ShowDialog();

        if (result == true)
        {
            string filename = saveFileDialog.FileName;

            await using FileStream writeStream = File.OpenWrite(filename);
            await JsonSerializer.SerializeAsync(writeStream, SelectedJourney!, serializationOptions);
        }
    }

    #endregion

    #region Protected Methods

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        SelectedJourney ??= ActiveJourneys.FirstOrDefault();

        return base.OnActivateAsync(cancellationToken);
    }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        string filename = Path.Combine("data", "data.json");
        await using FileStream openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model.Model>(openStream, cancellationToken: cancellationToken);
        wizardViewModel.Model = Model;

        serializationOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JourneyConverter(Model)
            }
        };
    }

    #endregion
}