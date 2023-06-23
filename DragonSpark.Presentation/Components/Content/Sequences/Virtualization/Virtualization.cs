using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Presentation.Environment.Browser.Document;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class Virtualization : ISelecting<InitializeInput, IJSObjectReference?>
{
	readonly LoadModule<Virtualization>        _load;
	readonly ICreateReference<InitializeInput> _initialize;

	public Virtualization(LoadModule<Virtualization> load) : this(load, Initialize.Default) {}

	public Virtualization(LoadModule<Virtualization> load, ICreateReference<InitializeInput> initialize)
	{
		_load       = load;
		_initialize = initialize;
	}

	public async ValueTask<IJSObjectReference?> Get(InitializeInput parameter)
	{
		var module   = new VirutalizationReference(new PolicyAwareJSObjectReference(await _load.Await()));
		var instance = await _initialize.Await(new(module, parameter));
		var result = instance.Account() is not null
			             ? new ConnectionAwareReference(new ModuleReference(module, instance), parameter.Reference)
			             : null;
		return result;
	}
}