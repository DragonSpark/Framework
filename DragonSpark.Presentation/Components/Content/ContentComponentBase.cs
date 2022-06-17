using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class ContentComponentBase<T> : ComponentBase
{
	First? first;

	[Inject]
	IActiveContents<T> Contents { get; set; } = default!;

	[Inject]
	CurrentRenderState Current { get; set; } = default!;

	protected override void OnInitialized()
	{
		var start = Start.A.Result<ValueTask<T?>>().By.Calling(GetContent).Out();
		Content = Contents.Get(new(this, start));

		base.OnInitialized();
	}

	protected IActiveContent<T> Content { get; private set; } = default!;

	protected abstract ValueTask<T?> GetContent();

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