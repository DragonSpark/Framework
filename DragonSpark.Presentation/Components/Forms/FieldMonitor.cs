﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public class FieldMonitor<T> : ComponentBase
	{
		[Parameter]
		public string FieldName { get; set; } = default!;

		/// <summary>Gets or sets a callback that updates the bound value.</summary>
		[Parameter]
		public EventCallback<T> Changed { get; set; }

		EditContext _editContext = default!;

		[CascadingParameter]
		EditContext EditContext
		{
			get => _editContext;
			set
			{
				if (_editContext != null)
				{
					_editContext.OnFieldChanged -= EditContextOnOnValidationRequested;
				}

				if ((_editContext = value) != null)
				{
					_editContext = value;
					_editContext.OnFieldChanged += EditContextOnOnValidationRequested;
				}

			}
		}

		void EditContextOnOnValidationRequested(object? sender, FieldChangedEventArgs args)
		{
			if (args.FieldIdentifier.FieldName == FieldName)
			{
				Changed.InvokeAsync(args.FieldIdentifier.GetValue<T>());
			}

		}
	}
}