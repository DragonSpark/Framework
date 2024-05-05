using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

public class FieldTypeMonitor<T> : ComponentBase
{
	[Parameter]
	public object? Model { get; set; }

	[Parameter]
	public string FieldName { get; set; } = default!;

	/// <summary>Gets or sets a callback that updates the bound value.</summary>
	[Parameter]
	public EventCallback<T> Changed { get; set; }

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
		var model = Model is null || Model == args.FieldIdentifier.Model || Model?.GetType() == args.FieldIdentifier.Model.GetType();
		if (model && args.FieldIdentifier.FieldName == FieldName)
		{
			Changed.Invoke(args.FieldIdentifier.GetValue<T>());
		}
	}
}