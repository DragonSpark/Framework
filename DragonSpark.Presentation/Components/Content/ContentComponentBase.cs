using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class ContentComponentBase<T> : ComponentBase
{
	readonly Func<ValueTask<T?>> _content;

	protected ContentComponentBase() => _content = GetContent;

	[Parameter, Inject]
	public IActiveContents<T> Contents { get; set; } = ActiveContents<T>.Default;

	protected override void OnInitialized()
	{
		_current = Create(Contents.Get(_content));
		base.OnInitialized();
	}

	protected IActiveContent<T> Content => _current.Verify();

	IActiveContent<T> _current = default!;

	First? first;

	protected abstract ValueTask<T?> GetContent();

	protected virtual IActiveContent<T> Create(IActiveContent<T> parameter)
		=> parameter.Then().Handle(Exceptions, GetType()).Get();

	protected virtual void RequestNewContent()
	{
		first = new();
	}

	protected override ValueTask RefreshState()
	{
		RequestNewContent();
		return base.RefreshState();
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> first?.Get() ?? false ? _current.Monitor.Get(StateChanged).AsTask() : base.OnAfterRenderAsync(firstRender);
}