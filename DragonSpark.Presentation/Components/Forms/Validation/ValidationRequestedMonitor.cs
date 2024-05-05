using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ValidationRequestedMonitor : ComponentBase, IDisposable
{
	readonly Switch _update = new();

	[Parameter]
	public EventCallback<EditContext> Requested { get; set; }

	[CascadingParameter]
	EditContext? EditContext
	{
		get => _context;
		set
		{
			if (_context != value)
			{
				if (_context != null)
				{
					_context.OnValidationRequested -= OnValidationRequested;
				}

				if ((_context = value) != null)
				{
					_context.OnValidationRequested += OnValidationRequested;
				}
			}
		}
	}	EditContext? _context;

	void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		Update();
	}

	void Update()
	{
		if (EditContext != null && _update.Up())
		{
			StateHasChanged();
		}
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> _update.Down() ? Requested.Invoke(EditContext) : base.OnAfterRenderAsync(firstRender);

	public void Dispose()
	{
		EditContext = null;
	}
}