using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DragonSpark.Application.Components.Validation;

sealed class Messages : ICommand<IEnumerable<ValidationResultMessage>>,
						ICommand<(FieldIdentifier Field, IEnumerable<ValidationResult> Results)>,
						ICommand
{
	readonly EditContext                          _edit;
	readonly ValidationMessageStore               _store;
	readonly ICollection<ValidationResultMessage> _messages;

	public Messages(EditContext edit, ValidationMessageStore store)
		: this(edit, store, new List<ValidationResultMessage>()) {}

	public Messages(EditContext edit, ValidationMessageStore store, ICollection<ValidationResultMessage> messages)
	{
		_edit     = edit;
		_store    = store;
		_messages = messages;
	}

	public void Execute(IEnumerable<ValidationResultMessage> parameter)
	{
		foreach (var message in parameter.AsValueEnumerable())
		{
			_messages.Add(message);
			_store.Add(message.Field, message.Message);
		}
	}

	public void Execute((FieldIdentifier Field, IEnumerable<ValidationResult> Results) parameter)
	{
		var (field, results) = parameter;
		_store.Clear(field);

		if (field.Model == _edit.Model)
		{
			var select = _messages.Introduce(field.FieldName)
								  .AsValueEnumerable()
								  .Where(x => x.Item1.Path.Equals(x.Item2))
								  .Select(x => x.Item1);
			foreach (var message in select.Any() ? select.ToArray() : Empty.Array<ValidationResultMessage>())
			{
				_store.Clear(message.Field);
				_messages.Remove(message);
			}
		}

		_store.Add(field, results.Where(x => x.ErrorMessage != null).Select(x => x.ErrorMessage.Verify()));
	}

	public void Execute(None parameter)
	{
		_store.Clear();
		_messages.Clear();
	}
}