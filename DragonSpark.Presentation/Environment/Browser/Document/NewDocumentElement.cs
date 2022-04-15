using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class NewDocumentElement : IAltering<IJSObjectReference>
{
	public static NewDocumentElement Default { get; } = new();

	NewDocumentElement() : this(nameof(NewDocumentElement)) {}

	readonly string _name;

	public NewDocumentElement(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(IJSObjectReference parameter)
		=> parameter.InvokeAsync<IJSObjectReference>(_name);
}