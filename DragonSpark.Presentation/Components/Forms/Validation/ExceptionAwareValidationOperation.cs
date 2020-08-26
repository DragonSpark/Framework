using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	sealed class ExceptionAwareValidationOperation : IOperation<ValidationContext>
	{
		readonly IOperation<ValidationContext> _previous;
		readonly ITemplate<(Type, string)>     _template;

		public ExceptionAwareValidationOperation(IOperation<ValidationContext> previous) :
			this(previous, Template.Default) {}

		public ExceptionAwareValidationOperation(IOperation<ValidationContext> previous,
		                                         ITemplate<(Type, string)> template)
		{
			_previous = previous;
			_template = template;
		}

		public async ValueTask Get(ValidationContext parameter)
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
}