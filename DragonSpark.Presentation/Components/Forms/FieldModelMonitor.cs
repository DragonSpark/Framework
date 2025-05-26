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
	public EventCallback Changed { get; set; }

	[CascadingParameter]
	EditContext? EditContext
	{
		get;
		set
		{
			if (field != value)
			{
				if (field is not null)
				{
					field.OnFieldChanged -= FieldChanged;
				}

				if ((field = value) != null)
				{
					field.OnFieldChanged += FieldChanged;
				}
			}
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs args)
	{
		var model = Model ?? args.FieldIdentifier.Model;
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