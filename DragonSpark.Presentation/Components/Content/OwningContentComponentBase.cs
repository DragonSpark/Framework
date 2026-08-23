using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content;

public abstract class OwningContentComponentBase<TService, TContent> : Scoped.OwningComponentBase<TService>
	where TService : class
{
	readonly Switch _load = true;

	protected override void OnInitialized()
	{
		var start = Start.A.Result<ValueTask<TContent?>>().By.Calling(GetContent).Out();
		Content = Contents.Get(new(this, start));

		base.OnInitialized();
	}


	[Inject]
	IActiveContents<TContent> Contents { get; set; } = null!;

	[Inject]
	RenderStateStore Current { get; set; } = null!;

	protected IActiveContent<TContent> Content { get; private set; } = null!;

	protected abstract ValueTask<TContent?> GetContent();

	protected virtual void RequestNewContent()
	{
		_load.Up();
	}

	protected override Task RefreshState()
	{
		if (Current.IsConnected())
		{
			RequestNewContent();
			return base.RefreshState();
		}
		return Task.CompletedTask;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (_load.Down())
		{
			Content.Execute();
		}
		base.OnAfterRender(firstRender);
	}
}