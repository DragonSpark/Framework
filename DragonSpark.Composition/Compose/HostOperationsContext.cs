using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Compose;

public sealed class HostOperationsContext
{
	readonly ISelecting<HostBuilder, IHost> _select;

	public HostOperationsContext(ISelecting<HostBuilder, IHost> select) => _select = select;

	public ValueTask<IHost> Run() => _select.Get(new HostBuilder());
}