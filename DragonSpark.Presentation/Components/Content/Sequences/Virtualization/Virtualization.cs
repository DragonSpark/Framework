using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Presentation.Environment.Browser.Document;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class Virtualization : IStopAware<InitializeInput, IJSObjectReference?>
{
	readonly LoadModule<Virtualization>        _load;
	readonly ICreateReference<InitializeInput> _initialize;

	public Virtualization(LoadModule<Virtualization> load) : this(load, Initialize.Default) {}

	public Virtualization(LoadModule<Virtualization> load, ICreateReference<InitializeInput> initialize)
	{
		_load       = load;
		_initialize = initialize;
	}

	public async ValueTask<IJSObjectReference?> Get(Stop<InitializeInput> parameter)
	{
		var ((_, disposable, _), stop) = parameter;
		var module       = new VirutalizationReference(new PolicyAwareJSObjectReference(await _load.Off(stop)));
		var instance     = await _initialize.Off(new(new(module, parameter), stop));
		var result = instance.Account() is not null
			             ? new ConnectionAwareReference(new ModuleReference(module, instance), disposable)
			             : null;
		return result;
	}
}