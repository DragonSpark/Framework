using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ModuleReference : IStopAware<string, IJSObjectReference>
{
	readonly IJSRuntime _runtime;
	readonly string     _import;

	public ModuleReference(IJSRuntime runtime, string import = "import")
	{
		_runtime = runtime;
		_import  = import;
	}

	public ValueTask<IJSObjectReference> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		return _runtime.InvokeAsync<IJSObjectReference>(_import, stop, subject);
	}
}