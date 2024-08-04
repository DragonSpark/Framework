using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class ContentComponentBase<T> : ComponentBase
{
	[Inject] IActiveContents<T> Contents { get; set; } = default!;

	[Inject] RenderStateStore Current { get; set; } = default!;

	protected override void OnInitialized()
	{
		Reset();

		base.OnInitialized();
	}

	protected void Reset()
	{
		var start = Start.A.Result<ValueTask<T?>>().By.Calling(GetContent).Out();
		Content = Contents.Get(new(this, start));
	}

	protected IActiveContent<T> Content { get; private set; } = default!;

	protected abstract ValueTask<T?> GetContent();

	protected virtual void RequestNewContent()
	{
		Content.Execute();
	}

	protected override ValueTask RefreshState()
	{
		switch (Current.Get())
		{
			case RenderState.Ready:
			case RenderState.Established:
				RequestNewContent();
				return base.RefreshState();
		}
		return ValueTask.CompletedTask;
	}
}