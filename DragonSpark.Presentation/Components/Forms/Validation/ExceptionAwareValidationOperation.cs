using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class ExceptionAwareValidationOperation : IStopAware<ValidationContext>
{
	readonly IStopAware<ValidationContext> _previous;
	readonly ITemplate<(Type, string)>     _template;

	public ExceptionAwareValidationOperation(IStopAware<ValidationContext> previous)
		: this(previous, Template.Default) {}

	public ExceptionAwareValidationOperation(IStopAware<ValidationContext> previous, ITemplate<(Type, string)> template)
	{
		_previous = previous;
		_template = template;
	}

	public async ValueTask Get(Stop<ValidationContext> parameter)
	{
		try
		{
			await _previous.On(parameter);
		}
		catch (Exception e)
		{
			var (((_, field), messages, (_, _, error)), _) = parameter;
			messages.Add(in field, error);
			throw _template.Get(e, field.Model.GetType(), field.FieldName);
		}
	}

	sealed class Template : ExceptionTemplate<Type, string>
	{
		public static Template Default { get; } = new();

		Template() : base("An exception occurred while performing an operation to validate '{Owner}.{Field}'.") {}
	}
}