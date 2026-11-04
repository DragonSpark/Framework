using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;
using Switch = DragonSpark.Model.Results.Switch;

namespace DragonSpark.Presentation.Components.Content.History;

public abstract class RecentContentComponentBase<T> : ComponentBase, IDisposable
{
	readonly  Switch _assigned = false;
	protected T?     _input;

	protected override void OnInitialized()
	{
		if (Recent.Get(new(this, Scope?.Scope)) is T i)
		{
			_assigned.Up();
			_input = i;	
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