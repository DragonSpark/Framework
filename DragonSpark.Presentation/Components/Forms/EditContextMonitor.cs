﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public class EditContextMonitor : ComponentBase, IDisposable
{
	[Parameter]
	public bool NotifyOnInitialized { get; set; }

	[Parameter]
	public EventCallback<EditContext> Changed { get; set; }

	[CascadingParameter]
	protected EditContext? EditContext
	{
		get => _context;
		set
		{
			if (_context != value)
			{
				if (_context != null)
				{
					_context.OnFieldChanged           -= FieldChanged;
					_context.OnValidationStateChanged -= ValidationStateChanged;
				}

				if ((_context = value) != null)
				{
					_context.OnFieldChanged           += FieldChanged;
					_context.OnValidationStateChanged += ValidationStateChanged;
				}
			}
		}
	}	EditContext? _context;

	protected override Task OnInitializedAsync()
		=> NotifyOnInitialized && EditContext != null ? Changed.InvokeAsync(EditContext) : base.OnInitializedAsync();

	void ValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
	{
		Update();
	}

	void Update()
	{
		if (EditContext != null)
		{
			Changed.InvokeAsync(EditContext);
		}
	}

	void FieldChanged(object? sender, FieldChangedEventArgs args)
	{
		Update();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		EditContext = null;
	}
}