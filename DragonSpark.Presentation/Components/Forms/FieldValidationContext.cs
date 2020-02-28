using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms {
	public sealed class FieldValidationContext : IOperation, ICommand
	{
		readonly FieldIdentifier                  _identifier;
		readonly Array<FieldValidationDefinition> _definitions;
		readonly ValidationMessageStore           _store;

		public FieldValidationContext(FieldIdentifier identifier, Array<FieldValidationDefinition> definitions,
		                              ValidationMessageStore store)
		{
			_identifier  = identifier;
			_definitions = definitions;
			_store       = store;
		}

		FieldValidationDefinition Current { get; set; }

		ValidationResult? Result { get; set; }

		public bool Active => Current != null;

		public string Text => Current?.LoadingText ??
		                      (Result.HasValue && !Result.Value.Valid ? Result.Value.Message : null);

		public async ValueTask Get()
		{
			try
			{
				foreach (var definition in _definitions.Open())
				{
					Current = definition;
					if (!await definition.Operation.Get(_identifier))
					{
						_store.Add(in _identifier, definition.ErrorText);
						Result = new ValidationResult(false, definition.ErrorText);
						return;
					}
				}

				Result = ValidationResult.Success;
			}
			finally
			{
				Current = null;
			}
		}

		public void Execute(None parameter)
		{
			_store.Clear(in _identifier);

			// TODO: Cancel?
			Result  = null;
			Current = null;
		}
	}
}