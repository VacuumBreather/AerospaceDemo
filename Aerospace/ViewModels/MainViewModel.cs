using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Aerospace.ViewModels;

internal class MainViewModel : Screen
{
    public Model Model { get; private set; }

    protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
    {
        var filename = Path.Combine("data", "data.json");
        await using var openStream = File.OpenRead(filename);
        Model = await JsonSerializer.DeserializeAsync<Model>(openStream, cancellationToken: cancellationToken);
    }
}