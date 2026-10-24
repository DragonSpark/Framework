using DragonSpark.Compose;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using Radzen;

namespace DragonSpark.Presentation.Components.Routing;

public class EditorComponent : NavigationAwareComponent
{
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var previous = EditContext;
		var changed  = parameters.DidParameterChange(nameof(EditContext), EditContext);
		await base.SetParametersAsync(parameters).Off();
		if (changed)
		{
			previous?.OnFieldChanged    -= OnChanged;
			EditContext?.OnFieldChanged += OnChanged;
		}
	}

	[CascadingParameter]
	EditContext? EditContext { get; set; }

	[Inject]
	IFocusHandler Focus { get; set; } = null!;

	void OnChanged(object? sender, FieldChangedEventArgs e)
	{
		if (HasChanges())
		{
			StateHasChanged();
		}
	}

	protected virtual bool HasChanges() => EditContext?.IsModified() == true;

	protected override async ValueTask<bool> Allow(LocationChangingContext parameter)
	{
		if (await Focus.GetFocusedElementAsync().On() is {} focused)
		{
			await focused.InvokeVoidAsync("blur").Off();
		}
		return !HasChanges();
	}

	protected override Task Exit()
	{
		EditContext?.MarkAsUnmodified();

		return base.Exit();
	}

	protected override void OnDispose(bool disposing)
	{
		base.OnDispose(disposing);
		EditContext = null!;
	}
}