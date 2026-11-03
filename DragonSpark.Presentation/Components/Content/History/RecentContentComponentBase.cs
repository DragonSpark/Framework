using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Templates;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.History;

public abstract class RecentContentComponentBase<T> : ComponentBase
{
	protected T? _input;

	protected override void OnInitialized()
	{
		if (History.Get(this) is T input)
		{
			_input = input;	
		}
		
		base.OnInitialized();
	}

	[Inject] HistoryTracker History { get; set; } = null!;

	[Parameter]
	public required T? Input { get; set; }

	[Parameter]
	public required RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

	[Parameter]
	public required RenderFragment<T> ChildContent { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Input), Input);
		await base.SetParametersAsync(parameters).Off();
		if (changed)
		{
			_input = Input;
			History.Execute(this, Input);
		}
	}
}