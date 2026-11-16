using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.History;

public abstract class RecentContentComponentBase<T> : ComponentBase, IDisposable
{
	protected override void OnInitialized()
	{
		if (Input is null && Recent.TryPop(new(this, Scope?.Scope), out var entry) && entry is T i)
		{
			Input = i;
		}
		
		base.OnInitialized();
	}

	[CascadingParameter] ContentScope? Scope { get; set; }

	[Inject] RecentContentTracker Recent { get; set; } = null!;

	[Parameter]
	public required T? Input { get; set; }

	[Parameter]
	public required RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

	[Parameter]
	public required RenderFragment<T> ChildContent { get; set; }

	public void Dispose()
	{
		Recent.Execute(new(this, Scope?.Scope), Input);
	}
}