using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class ExceptionAwareValidationOperation : IOperation<Validation>
	{
		readonly IOperation<Validation>    _previous;
		readonly ITemplate<(Type, string)> _template;

		public ExceptionAwareValidationOperation(IOperation<Validation> previous) : this(previous, Template.Default) {}

		public ExceptionAwareValidationOperation(IOperation<Validation> previous, ITemplate<(Type, string)> template)
		{
			_previous = previous;
			_template = template;
		}

		public async ValueTask Get(Validation parameter)
		{
			try
			{
				await _previous.Await(parameter);
			}
			catch (Exception e)
			{
				var ((_, field), messages, (_, _, error)) = parameter;
				messages.Add(in field, error);
				throw _template.Get(e, field.Model.GetType(), field.FieldName);
			}
		}

		sealed class Template : ExceptionTemplate<Type, string>
		{
			public static Template Default { get; } = new Template();

			Template() : base("An exception occurred while performing an operation to validate '{Owner}.{Field}'.") {}
		}
	}

	sealed class ValidationOperation<T> : IOperation<Validation>
	{
		readonly IFieldValidation<T> _validator;

		public ValidationOperation(IFieldValidation<T> validator) => _validator = validator;

		public async ValueTask Get(Validation parameter)
		{
			var ((_, field), messages, (invalid, _, _)) = parameter;
			if (!await _validator.Await(field.GetValue<T>()))
			{
				messages.Add(in field, invalid);
			}
		}
	}

	public interface IFieldValidation<in T> : IDepending<T> {}

	public readonly struct Validation
	{
		public Validation(FieldContext context, ValidationMessageStore messages, FieldValidationMessages messaging)
		{
			Context   = context;
			Messages  = messages;
			Messaging = messaging;
		}

		public FieldContext Context { get; }

		public ValidationMessageStore Messages { get; }
		public FieldValidationMessages Messaging { get; }

		public void Deconstruct(out FieldContext context, [NotNull] out ValidationMessageStore messages,
		                        [NotNull] out FieldValidationMessages messaging)
		{
			context   = Context;
			messages  = Messages;
			messaging = Messaging;
		}
	}

	public readonly struct FieldContext
	{
		public FieldContext(EditContext context, FieldIdentifier field)
		{
			Context = context;
			Field   = field;
		}

		public EditContext Context { get; }

		public FieldIdentifier Field { get; }

		public void Deconstruct([NotNull] out EditContext context, out FieldIdentifier field)
		{
			context = Context;
			field   = Field;
		}
	}

	public class ValidationOperation : ComponentBase, IDisposable
	{
		ValidationMessageStore _messages = default!;
		EditContext?           _context;

		[Parameter]
		public FieldIdentifier Identifier { get; set; }

		[Parameter]
		public string Message { get; set; } = "This field does not contain a valid value.";

		[Parameter]
		public EventCallback<Validation> Validate { get; set; }

		[CascadingParameter]
		EditContext? Context
		{
			get => _context;
			set
			{
				if (_context != value)
				{
					if (_context != null)
					{
						_messages.Clear();
						_context.OnFieldChanged -= ContextOnOnFieldChanged;
					}

					if ((_context = value) != null)
					{
						_messages  = new ValidationMessageStore(_context);

						_context.OnFieldChanged += ContextOnOnFieldChanged;
					}
				}
			}
		}

		void ContextOnOnFieldChanged(object? sender, FieldChangedEventArgs e)
		{
			if (e.FieldIdentifier.Equals(Identifier))
			{
				InvokeAsync(Update);
			}
		}

		async Task Update()
		{
			_messages.Clear(Identifier);

			var context    = new FieldContext(Context.Verify(), Identifier);
			var validation = new Validation(context, _messages, Message);

			await Validate.InvokeAsync(validation);

			_context.Verify().NotifyValidationStateChanged();
		}

		public virtual void Dispose()
		{
			Context = null;
		}
	}
}