using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class FieldValidationContext : IOperation, ICommand
	{
		readonly FieldValidator                   _owner;
		readonly Array<FieldValidationDefinition> _definitions;
		readonly ValidationMessageStore           _store;
		FieldValidationDefinition                 _current;

		public FieldValidationContext(FieldValidator owner, Array<FieldValidationDefinition> definitions,
		                              EditContext context)
			: this(owner, definitions, new ValidationMessageStore(context)) {}

		public FieldValidationContext(FieldValidator owner, Array<FieldValidationDefinition> definitions,
		                              ValidationMessageStore store)
		{
			_owner       = owner;
			_definitions = definitions;
			_store       = store;
		}

		FieldValidationDefinition Current
		{
			get => _current;
			set
			{
				var refresh = value != null && _current != null;
				_current = value;
				if (refresh)
				{
					_owner.Execute();
				}
			}
		}

		ValidationResult? Result { get; set; }

		public bool? Valid => Current == null ? Result.HasValue && Result.Value.Valid : (bool?)null;

		public string Text => Current?.Messages.Loading ??
		                      (Result.HasValue && !Result.Value.Valid ? Result.Value.Message : null);

		public async ValueTask Get()
		{
			try
			{
				foreach (var definition in _definitions.Open())
				{
					Current = definition;
					if (!await definition.Operation.Get(_owner.Identifier))
					{
						Invalidate(definition.Messages.Invalid);
						return;
					}
				}

				Result = ValidationResult.Success;
			}
			// ReSharper disable once CatchAllClause
			catch (Exception error)
			{
				Invalidate(Current.Messages.Error);
				_owner.Logger.LogError(error,
				                       "An exception occurred while performing a long-running validation operation on field '{Field}' using operation of type '{Operation}'.",
				                       _owner.Identifier.FieldName, Current.Operation.GetType());
			}
			finally
			{
				Current = null;
			}
		}

		void Invalidate(string message)
		{
			Result = new ValidationResult(false, message);
			_store.Add(_owner.Identifier, message);
		}

		public void Execute(None parameter)
		{
			_store.Clear(_owner.Identifier);

			// TODO: Cancel?
			Result  = null;
			Current = null;
		}
	}
}