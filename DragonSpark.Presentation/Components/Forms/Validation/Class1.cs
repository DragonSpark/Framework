using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public static class Extensions
	{
		public static IValidateValue<object> Validator(this IExpression @this)
			=> new RegularExpressionValidator(@this.Get());

		public static BoundedExpression Bounded(this IExpression @this) => new BoundedExpression(@this.Get());
	}

	public sealed class ScreenNamePattern : Expression
	{
		public static ScreenNamePattern Default { get; } = new ScreenNamePattern();

		ScreenNamePattern() : base("[a-zA-Z0-9_]") {}
	}

	public sealed class DisplayNamePattern : Expression
	{
		public static DisplayNamePattern Default { get; } = new DisplayNamePattern();

		DisplayNamePattern() : base("[a-zA-Z0-9- _]") {}
	}

	public class RegularExpressionValidator : MetadataValueValidator
	{
		public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

		public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
	}

	/*public sealed class DisplayNameValidation : ISelect<uint, Expression>
	{
		public static DisplayNameValidation Default { get; } = new DisplayNameValidation();

		DisplayNameValidation() : this(DisplayNamePattern.Default) {}

		readonly string _pattern;

		public DisplayNameValidation(string pattern) => _pattern = pattern;

		public Expression Get(uint parameter) => new Expression(string.Format(_pattern, parameter.ToString()));
	}*/

	public sealed class RequiredValidator : MetadataValueValidator
	{
		public static RequiredValidator Default { get; } = new RequiredValidator();

		RequiredValidator() : base(new RequiredAttribute()) {}
	}

	public sealed class EmailAddressValidator : RegularExpressionValidator
	{
		public static EmailAddressValidator Default { get; } = new EmailAddressValidator();

		EmailAddressValidator() : base(EmailAddressExpression.Default) {}
	}

	public class MetadataValueValidator : Condition<object>, IValidateValue<object>
	{
		public MetadataValueValidator(ValidationAttribute metadata) : base(metadata.IsValid) {}
	}
	
	public class GeneralFieldValidator : FieldValidation<object> {}

	// TODO: name.
	public class FieldValidation<T> : ComponentBase, IDisposable
	{
		ValidationMessageStore _messages = default!;
		EditContext?           _context;

		[Parameter]
		public FieldIdentifier Identifier { get; set; }

		[Parameter]
		public IValidateValue<T> Validator { get; set; } = default!;

		[Parameter]
		public string ErrorMessage { get; set; } = "This field does not contain a valid value.";

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
						_messages.Clear();
						_context.OnFieldChanged        -= FieldChanged;
						_context.OnValidationRequested -= ValidationRequested;
					}

					if ((_context = value) != null)
					{
						_messages = new ValidationMessageStore(_context);

						_context.OnFieldChanged        += FieldChanged;
						_context.OnValidationRequested += ValidationRequested;
					}
				}
			}
		}

		void ValidationRequested(object? sender, ValidationRequestedEventArgs e)
		{
			if (!_messages[Identifier].AsValueEnumerable().Any())
			{
				Update();
			}
		}

		void FieldChanged(object? sender, FieldChangedEventArgs e)
		{
			if (e.FieldIdentifier.Equals(Identifier))
			{
				Update();
			}
		}

		void Update()
		{
			_messages.Clear(Identifier);
			var valid = Validator.Get(Identifier.GetValue<T>());
			if (!valid)
			{
				_messages.Add(Identifier, ErrorMessage);
			}

			_context.Verify().NotifyValidationStateChanged();
		}

		public void Dispose()
		{
			EditContext = null;
		}
	}

	public interface IValidatingValue<in T> : IDepending<T> {}


	public interface IValidateValue<in T> : ICondition<T> {}

	public interface IExpression : IResult<string> {}

	public class Expression : Text.Text, IExpression
	{
		public Expression([NotNull] string instance) : base(instance) {}
	}

	public sealed class BoundedExpression : ISelect<Bounds, Expression>
	{
		readonly string _expression;

		public BoundedExpression(string expression) => _expression = expression;

		public Expression Get(Bounds parameter) => new Expression($"^{_expression}{{{parameter.Minimum},{parameter.Maximum}}}$");
	}

	public readonly struct Bounds
	{
		public Bounds(uint minimum, uint maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public uint Minimum { get; }

		public uint Maximum { get; }
	}

	public sealed class EmailAddressExpression : Expression
	{
		public static EmailAddressExpression Default { get; } = new EmailAddressExpression();

		EmailAddressExpression() : base(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,9})$") {}
	}

	public sealed class StandardCharactersPattern : Expression
	{
		public static StandardCharactersPattern Default { get; } = new StandardCharactersPattern();

		StandardCharactersPattern() : base(@"([a-zA-Z0-9- _.;'"":!+*&%$#^@=[\]()~`<>\\/,?{}\|]|\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])") {}
	}

	public sealed class ValidUrlExpression : Expression
	{
		public static ValidUrlExpression Default { get; } = new ValidUrlExpression();

		ValidUrlExpression() : base(@"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png)$") {}
	}

	public interface IOperations : ICommand<Task>, ICommand, ICondition, IOperation {}

	sealed class Operations : IOperations
	{
		readonly IList<Task> _tasks;

		public Operations() : this(new List<Task>()) {}

		public Operations(IList<Task> tasks) => _tasks = tasks;

		public void Execute(Task parameter)
		{
			_tasks.Add(parameter);
		}

		public ValueTask Get() => Task.WhenAll(_tasks).ToOperation();

		public void Execute(None parameter)
		{
			_tasks.Clear();
		}

		public bool Get(None parameter) => _tasks.Count > 0;
	}

	public interface IOperationsStore : ISelect<EditContext, IOperations> {}

	sealed class OperationsStore : ReferenceValueStore<EditContext, IOperations>, IOperationsStore
	{
		public static OperationsStore Default { get; } = new OperationsStore();

		OperationsStore() : base(_ => new Operations()) {}
	}

	sealed class ValidContext : IDepending<EditContext>
	{
		static async ValueTask Process(IOperations list)
		{
			if (A.Condition(list).Get())
			{
				var awaitable = list.Await();
				list.Execute();
				await awaitable;
			}
		}

		public static ValidContext Default { get; } = new ValidContext();

		ValidContext() : this(OperationsStore.Default.Get) {}

		readonly Func<EditContext, IOperations> _list;

		public ValidContext(Func<EditContext, IOperations> list) => _list = list;

		public async ValueTask<bool> Get(EditContext parameter)
		{
			var list = _list(parameter);
			if (parameter.Validate())
			{
				await Process(list).ConfigureAwait(false);

				return parameter.IsValid();
			}

			if (A.Condition(list).Get())
			{
				list.Execute();
			}

			return false;
		}
	}

	sealed class Submit : IOperation<EditContext>
	{
		readonly IOperation<EditContext>  _operation;
		readonly Operate<EditContext, bool> _valid;

		public Submit(IOperation<EditContext> operation) : this(operation, ValidContext.Default.Get) {}

		public Submit(IOperation<EditContext> operation, Operate<EditContext, bool> valid)
		{
			_operation = operation;
			_valid     = valid;
		}

		public async ValueTask Get(EditContext parameter)
		{
			if (await _valid(parameter))
			{
				await _operation.Get(parameter);
				parameter.MarkAsUnmodified();
			}
		}
	}
}