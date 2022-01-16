using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

public class EditorComponent : ChangeAwareComponent
{
	public override bool HasChanges => Session.ActiveComponent == this && EditContext.IsModified();

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

	void OnChanged(object? sender, FieldChangedEventArgs e)
	{
		if (EditContext.IsModified(e.FieldIdentifier))
		{
			InvokeAsync(Session.UpdateExitState().AsTask);
		}
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