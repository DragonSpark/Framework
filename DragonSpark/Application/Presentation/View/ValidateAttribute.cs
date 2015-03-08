using System;

namespace DragonSpark.Application.Presentation.View
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class ValidateAttribute : Attribute
	{
		/*protected override bool CanExecute(ActionExecutionContext context)
		{
			var target = context.Target;
			var result = Validator.TryValidateObject( target, new ValidationContext( target, null, null ), null );
			return result;
		}*/
	}
}