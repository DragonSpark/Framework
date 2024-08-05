using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public abstract class FieldMonitorBase : ComponentBase, IDisposable
{
	readonly Switch _update = new();

	[Parameter]
	public object? Model { get; set; }

	[Parameter]
	public string FieldName { get; set; } = default!;

	[Parameter]
	public EventCallback Updated { get; set; }

	protected FieldIdentifier Identifier { get; private set; }

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