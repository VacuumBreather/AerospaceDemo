using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class MainViewModel : Conductor<Screen>
{
    private readonly WizardViewModel _wizardViewModel;

    public MainViewModel(WizardViewModel wizardViewModel)
    {
        _wizardViewModel = wizardViewModel;
    }

    public Model.Model Model { get; private set; }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        var filename = Path.Combine("data", "data.json");
        await using var openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model.Model>(openStream, cancellationToken: cancellationToken);
    }

    protected override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _wizardViewModel.Model = Model;

        ActiveItem = _wizardViewModel;

        return base.OnActivateAsync(cancellationToken);
    }
}