using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
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

	[Inject]
	IContentInteraction Interaction { get; set; } = default!;

	protected IActiveContent<T> Content => _current.Verify();

	IActiveContent<T>? _current;

	protected abstract ValueTask<T?> GetContent();

	protected override void OnParametersSet()
	{
		Apply();
	}

	void Apply()
	{
		_current ??= Create(Contents.Get(_content));
	}

	protected virtual IActiveContent<T> Create(IActiveContent<T> parameter)
		=> parameter.Then().Handle(Exceptions, GetType()).Get();

	protected void RequestNewContent()
	{
		_current = null;
	}

	protected override ValueTask RefreshState()
	{
		Interaction.Execute();
		RequestNewContent();
		Apply();
		return base.RefreshState();
	}
}