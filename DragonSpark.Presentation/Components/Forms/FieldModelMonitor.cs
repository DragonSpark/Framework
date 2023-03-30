using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

public class FieldModelMonitor : ComponentBase
{
	[Parameter]
	public object Model { get; set; } = default!;

	[Parameter]
	public EventCallback Changed { get; set; }

	EditContext? _editContext;

	[CascadingParameter]
	EditContext? EditContext
	{
		get => _editContext;
		set
		{
			if (_editContext != value)
			{
				if (_editContext != null)
				{
					_editContext.OnFieldChanged -= FieldChanged;
				}

				if ((_editContext = value) != null)
				{
					_editContext.OnFieldChanged += FieldChanged;
				}
			}
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs args)
	{
		var model = Model.Account() ?? EditContext?.Model;
		if (args.FieldIdentifier.Model == model)
		{
			Changed.InvokeAsync(Model);
		}
	}
}