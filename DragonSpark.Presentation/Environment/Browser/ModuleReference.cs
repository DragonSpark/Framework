using DragonSpark.Model.Operations.Selection;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ModuleReference : ISelecting<string, IJSObjectReference>
{
	readonly IJSRuntime _runtime;
	readonly string     _import;

	public ModuleReference(IJSRuntime runtime, string import = "import")
	{
		_runtime = runtime;
		_import  = import;
	}

	public ValueTask<IJSObjectReference> Get(string parameter)
		=> _runtime.InvokeAsync<IJSObjectReference>(_import, parameter);
}