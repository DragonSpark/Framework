using DragonSpark.Compose;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

public class EditorComponent : NavigationAwareComponent
{
	[CascadingParameter]
	EditContext EditContext
	{
		get => _editContext;
		set
		{
			if (_editContext != value)
			{
				if (_editContext.Account() != null)
				{
					_editContext.OnFieldChanged -= OnChanged;
				}

				if ((_editContext = value) != null)
				{
					_editContext.OnFieldChanged += OnChanged;
				}
			}
		}
	}	EditContext _editContext = default!;

	[Inject]
	IFocusHandler Focus { get; set; } = default!;

	void OnChanged(object? sender, FieldChangedEventArgs e)
	{
		if (EditContext.IsModified(e.FieldIdentifier))
		{
//			InvokeAsync(() => Session.UpdateExitState().AsTask());
		}
	}

	protected virtual bool HasChanges() => EditContext.IsModified();

	protected override async ValueTask<bool> Allow(LocationChangingContext parameter)
	{
		var focused = await Focus.GetFocusedElementAsync();
		if (focused.Account() is not null)
		{
			await focused.InvokeVoidAsync("blur").ConfigureAwait(false);
		}
		return !HasChanges();
	}

	protected override Task Exit()
	{
		EditContext.MarkAsUnmodified();

		return base.Exit();
	}

	protected override void OnDispose(bool disposing)
	{
		base.OnDispose(disposing);
		EditContext = null!;
	}
}