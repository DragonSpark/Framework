using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.History;

public abstract class RecentContentComponentBase<T> : ComponentBase, IDisposable
{
	protected T? _input;

	protected override void OnInitialized()
	{
		if (Recent.TryPop(new(this, Scope?.Scope), out var entry) && entry is T i)
		{
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

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Input), Input) ;
		await base.SetParametersAsync(parameters).Off();
		if (changed && Input is not null)
		{
			_input = default;
		}
	}

	public void Dispose()
	{
		Recent.Execute(new(this, Scope?.Scope), Input);
	}
}