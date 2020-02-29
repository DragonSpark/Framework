using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Forms;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation
{
	public static class Extensions
	{
		public static OperationView<TIn, TOut> AsView<TIn, TOut>(this IOperationResult<TIn, TOut> @this)
			=> new OperationView<TIn, TOut>(@this);

		public static OperationView<T> AsView<T>(this IOperationResult<T> @this) => new OperationView<T>(@this);

		// ReSharper disable once TooManyArguments
		public static FieldValidationDefinition AsDefinition(this IOperationResult<FieldIdentifier, bool> @this,
		                                                     string invalid, string loading = "Loading...",
		                                                     string error =
			                                                     "There was a problem validating this field.")
			=> new FieldValidationDefinition(@this, new FieldValidationMessages(invalid, loading, error));

		public static T GetValue<T>(this FieldIdentifier @this) => @this.Model.GetType()
		                                                                .GetProperty(@this.FieldName)
		                                                                .GetValue(@this.Model)
		                                                                .To<T>(); // TODO: Optimize with delegate.
	}
}