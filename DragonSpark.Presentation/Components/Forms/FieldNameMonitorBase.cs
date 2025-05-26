using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public abstract class FieldNameMonitorBase : ComponentBase, IDisposable
{
	readonly Switch _update = new();

	[Parameter]
	public object? Model { get; set; }

	[Parameter]
	public string FieldName { get; set; } = null!;

	[Parameter]
	public EventCallback Updated { get; set; }

	protected FieldIdentifier Identifier { get; private set; }

	[CascadingParameter]
	EditContext? EditContext
	{
		get;
		set
		{
			if (field != value)
			{
				if (field != null)
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
		var model = Model is null || Model == args.FieldIdentifier.Model;
		if (model && args.FieldIdentifier.FieldName == FieldName && _update.Up())
		{
			Identifier = args.FieldIdentifier;
			StateHasChanged();
		}
	}

	protected virtual Task OnUpdate() => Updated.Invoke();

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> _update.Down() ? OnUpdate() : base.OnAfterRenderAsync(firstRender);

	public void Dispose()
	{
		EditContext = null;
	}

}