using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class EditorValidation : IResulting<bool>
{
	readonly EditContext                                     _subject;
	readonly object                                          _sender;
	readonly Func<EventHandler<ValidationCallbackEventArgs>> _validated;

	public EditorValidation(EditContext subject, object sender,
	                        Func<EventHandler<ValidationCallbackEventArgs>> validated)
	{
		_subject   = subject;
		_sender    = sender;
		_validated = validated;
	}

	public async ValueTask<bool> Get()
	{
		var result = _subject.Validate();
		if (result)
		{
			var args = new ValidationCallbackEventArgs();
			_validated()(_sender, args);
			foreach (var callback in args.Callbacks.AsValueEnumerable())
			{
				await callback.Invoke().On();
			}
		}

		return result;
	}
}