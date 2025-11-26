using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ModelValidationSummary : ComponentBase, IDisposable
{
	ValidationMessageStore? _store;

	[Parameter]
	public ValidationFieldMessage? Message { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var change  = parameters.DidParameterChange(nameof(Message), Message);
		await base.SetParametersAsync(parameters).On();
		if (change && _store is not null)
		{
			_store.Clear();

			if (Message is not null)
			{
				_store.Add(Message.Field, Message.Message);
				EditContext?.NotifyValidationStateChanged();
			}
		}
	}

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
					_store?.Clear();
					field.OnFieldChanged        -= Update;
					field.OnValidationRequested -= Update;
				}

				if ((field = value) != null)
				{
					Assign(field);
				}
			}
		}
	}

	void Assign(EditContext parameter)
	{
		parameter.OnFieldChanged        += Update;
		parameter.OnValidationRequested += Update;

		_store = new ValidationMessageStore(parameter);
	}

	void Update(object? sender, EventArgs e)
	{
		Update();
	}

	void Update()
	{
		if (EditContext != null)
		{
			_store?.Clear();
		}
	}

	public void Dispose()
	{
		EditContext = null;
	}
}