using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Browser;

public class DocumentInitializer : RadzenComponent
{
	Func<Task> _execute = default!;

	bool Rendered { get; set; }

	[Parameter]
	public string Method { get; set; } = "_initialize";

	[Inject]
	IJSRuntime Runtime { get; set; } = default!;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_execute = OnReady;
	}

	Task OnReady() => Runtime.InvokeVoidAsync(Method).AsTask();

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			StateHasChanged();
		}
		else if (!Rendered)
		{
			Rendered = true;
			Debounce(_execute);
		}
	}
}