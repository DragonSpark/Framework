using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class OwningContentComponentBase<TService, TContent> : Scoped.OwningComponentBase<TService>
	where TService : class
{
	protected override void OnInitialized()
	{
		var start = Start.A.Result<ValueTask<TContent?>>().By.Calling(GetContent).Out();
		Content = Contents.Get(new(this, start));

		base.OnInitialized();
	}


	[Inject]
	IActiveContents<TContent> Contents { get; set; } = default!;

	[Inject]
	RenderStateStore Current { get; set; } = default!;

	protected IActiveContent<TContent> Content { get; private set; } = default!;

	protected abstract ValueTask<TContent?> GetContent();

	First? first;
	protected virtual void RequestNewContent()
	{
		first = new();
	}

	protected override ValueTask RefreshState()
	{
		switch (Current.Get())
		{
			case RenderState.Default:
				break;
			default:
				RequestNewContent();
				return base.RefreshState();
		}
		return ValueTask.CompletedTask;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (first?.Get() ?? false)
		{
			Content.Execute();
		}
		base.OnAfterRender(firstRender);
	}
}