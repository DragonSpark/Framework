using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class FieldModelMonitor : ComponentBase, IDisposable
{
	readonly Switch _update = new();

	[Parameter]
	public object? Model { get; set; }

	[Parameter]
	public bool AssumeEditContextModel { get; set; } = true;

	[Parameter]
	public EventCallback Changed { get; set; }

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
	}	EditContext? _editContext;

	void FieldChanged(object? sender, FieldChangedEventArgs args)
	{
		var model = Model ?? (AssumeEditContextModel ? EditContext?.Model : args.FieldIdentifier.Model);
		if (args.FieldIdentifier.Model == model && _update.Up())
		{
			StateHasChanged();
		}
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> _update.Down() ? Changed.Invoke() : base.OnAfterRenderAsync(firstRender);

	public void Dispose()
	{
		EditContext = null;
	}
}