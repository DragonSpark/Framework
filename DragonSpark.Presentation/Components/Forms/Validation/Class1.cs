﻿using DragonSpark.Compose;
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

	}

	sealed class DisplayNamePattern : Text.Text
	{
		public static DisplayNamePattern Default { get; } = new DisplayNamePattern();

		DisplayNamePattern() : base("^[a-zA-Z0-9- _]{{1,{0}}}$") {}
	}

	public class RegularExpressionValidator : MetadataValueValidator
	{
		public RegularExpressionValidator(string expression) : this(new RegularExpressionAttribute(expression)) {}

		public RegularExpressionValidator(RegularExpressionAttribute metadata) : base(metadata) {}
	}

	public sealed class DisplayNameValidation : ISelect<uint, Expression>
	{
		public static DisplayNameValidation Default { get; } = new DisplayNameValidation();

		DisplayNameValidation() : this(DisplayNamePattern.Default) {}

		readonly string _pattern;

		public DisplayNameValidation(string pattern) => _pattern = pattern;

		public Expression Get(uint parameter) => new Expression(string.Format(_pattern, parameter.ToString()));
	}

	public sealed class RequiredValidator : MetadataValueValidator
	{
		public static RequiredValidator Default { get; } = new RequiredValidator();

		RequiredValidator() : base(new RequiredAttribute()) {}
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

	public interface IValidate : ICondition<ValidationContext> {}

	public interface IValidateValue<in T> : ICondition<T> {}

	public interface IExpression : IResult<string> {}

	public class Expression : Text.Text, IExpression
	{
		public Expression([NotNull] string instance) : base(instance) {}
	}

	public sealed class ValidUrlExpression : Expression
	{
		public const string Pattern = @"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png)$";

		public static ValidUrlExpression Default { get; } = new ValidUrlExpression();

		ValidUrlExpression() : base(Pattern) {}
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
		readonly Await<EditContext, bool> _valid;

		public Submit(IOperation<EditContext> operation) : this(operation, ValidContext.Default.Await) {}

		public Submit(IOperation<EditContext> operation, Await<EditContext, bool> valid)
		{
			_operation = operation;
			_valid     = valid;
		}

		public async ValueTask Get(EditContext parameter)
		{
			if (await _valid(parameter))
			{
				await _operation.Await(parameter);
			}
		}
	}
}