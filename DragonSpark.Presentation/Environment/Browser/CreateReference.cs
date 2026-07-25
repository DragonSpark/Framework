using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser;

public class CreateReference<T> : ICreateReference<T> where T : IArray<object>
{
	readonly string _name;

	protected CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(Stop<CreateReferenceInput<T>> parameter)
	{
		var ((reference, input), stop) = parameter;
		return reference.InvokeAsync<IJSObjectReference>(_name, stop, input.Get().Open());
	}
}

public class CreateReference : IAltering<IJSObjectReference>
{
	readonly string _name;

	protected CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(Stop<IJSObjectReference> parameter)
	{
		var (subject, stop) = parameter;
		return subject.InvokeAsync<IJSObjectReference>(_name, stop);
	}
}