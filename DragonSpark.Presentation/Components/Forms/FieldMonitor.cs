using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class FieldMonitor<T> : ComponentBase, IDisposable
{
	readonly Switch _update = new();

	FieldIdentifier _identifier;

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
		var model = Model is null || Model == args.FieldIdentifier.Model;
		if (model && args.FieldIdentifier.FieldName == FieldName && _update.Up())
		{
			_identifier = args.FieldIdentifier;
			StateHasChanged();
		}
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if (_update.Down())
		{
			var value = _identifier.GetValue<T>();
			return Changed.Invoke(value);
		}

		return base.OnAfterRenderAsync(firstRender);
	}

	public void Dispose()
	{
		EditContext = null;
	}

}