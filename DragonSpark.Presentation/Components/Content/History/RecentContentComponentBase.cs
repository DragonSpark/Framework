using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;
using Radzen;
using Switch = DragonSpark.Model.Results.Switch;

namespace DragonSpark.Presentation.Components.Content.History;

public abstract class RecentContentComponentBase<T> : ComponentBase
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

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Input), Input) ;
		await base.SetParametersAsync(parameters).Off();
		if ((!_assigned || !_assigned.Down()) && changed)
		{
			_input = Input;
			Recent.Execute(new(this, Scope?.Scope), Input);
		}
	}
}