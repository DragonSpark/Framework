using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State.Resize;

sealed class ResizeMonitorInstance : IAltering<IJSObjectReference>
{
	public static ResizeMonitorInstance Default { get; } = new();

	ResizeMonitorInstance() : this(nameof(ResizeMonitorInstance)) {}

	readonly string _name;

	public ResizeMonitorInstance(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(IJSObjectReference parameter)
		=> parameter.InvokeAsync<IJSObjectReference>(_name);
}