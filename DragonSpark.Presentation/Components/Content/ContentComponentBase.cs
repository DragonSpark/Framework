using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class ContentComponentBase<T> : ComponentBase
{
	[Inject] IActiveContents<T> Contents { get; set; } = null!;

	[Inject] RenderStateStore Current { get; set; } = null!;

	protected override void OnInitialized()
	{
		Reset();

		base.OnInitialized();
	}

	protected void Reset()
	{
		var start = Start.A.Result(GetContent).Out();
		Content = Contents.Get(new(this, start));
	}

	protected IActiveContent<T> Content { get; private set; } = null!;

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