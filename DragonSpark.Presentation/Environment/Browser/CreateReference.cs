using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public interface ICreateReference<T> : ISelecting<CreateReferenceInput<T>, IJSObjectReference> where T : IArray<object>;

public class CreateReference<T> : ICreateReference<T> where T : IArray<object>
{
	readonly string _name;

	protected CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(CreateReferenceInput<T> parameter)
	{
		var (reference, input) = parameter;
		return reference.InvokeAsync<IJSObjectReference>(_name, input.Get().Open());
	}
}

public class CreateReference : IAltering<IJSObjectReference>
{
	readonly string _name;

	protected CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(IJSObjectReference parameter)
		=> parameter.InvokeAsync<IJSObjectReference>(_name);
}