using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public class ValidationRequestedMonitor : ComponentBase, IDisposable
{
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
		if (EditContext != null)
		{
			Requested.InvokeAsync(EditContext);
		}
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		EditContext = null;
	}
}