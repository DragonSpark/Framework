using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class FieldValidationContext : IOperation<FieldValidator>, ICommand<FieldIdentifier>
	{
		readonly IValidationDefinition  _view;
		readonly ValidationMessageStore _store;

		public FieldValidationContext(IValidationDefinition view, EditContext context)
			: this(view, new ValidationMessageStore(context)) {}

		public FieldValidationContext(IValidationDefinition view, ValidationMessageStore store)
		{
			_view  = view;
			_store = store;
		}

		ValidationResult? Result { get; set; }

		public bool? Valid => _view.IsActive ? null : Result?.Valid;

		public string Text => _view.IsActive
			                      ? _view.Messages.Loading
			                      : Result.HasValue && !Result.Value.Valid
				                      ? Result?.Message
				                      : null;

		public async ValueTask Get(FieldValidator parameter)
		{
			try
			{
				var result = await _view.Get(parameter);
				if (!result.Valid)
				{
					Invalidate(parameter.Identifier, result.Message);
					return;
				}

				Result = ValidationResult.Success;
			}
			// ReSharper disable once CatchAllClause
			catch (Exception error)
			{
				Invalidate(parameter.Identifier, _view.Messages.Error);
				parameter.Logger.LogError(error,
				                          "An exception occurred while performing an operation to validate '{Field}'.",
				                          parameter.Identifier.FieldName);
			}
		}

		void Invalidate(FieldIdentifier identifier, string message)
		{
			Result = new ValidationResult(false, message);
			_store.Add(identifier, message);
		}

		public void Execute(FieldIdentifier parameter)
		{
			Result = null;
			_store.Clear(parameter);
		}
	}
}