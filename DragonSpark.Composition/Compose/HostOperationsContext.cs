using System.Threading.Tasks;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public sealed class HostOperationsContext
{
    readonly ISelecting<HostBuilder, IHost> _select;

    public HostOperationsContext(ISelecting<HostBuilder, IHost> select) => _select = select;

    [MustDisposeResource]
    public ValueTask<IHost> Run() => _select.Get(new());
}
