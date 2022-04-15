using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class DocumentElement : IAsyncDisposable
{
	readonly IJSObjectReference _instance;
	readonly IOperation         _dispose;

	public DocumentElement(IJSObjectReference instance) : this(instance, new DisposeReference(instance)) {}

	public DocumentElement(IJSObjectReference instance, IOperation dispose)
	{
		_instance = instance;
		_dispose  = dispose;
	}

	public ValueTask AddClass(string name) => _instance.InvokeVoidAsync(nameof(AddClass), name);

	public ValueTask RemoveClass(string name) => _instance.InvokeVoidAsync(nameof(RemoveClass), name);

	public ValueTask DisposeAsync() => _dispose.Get();
}