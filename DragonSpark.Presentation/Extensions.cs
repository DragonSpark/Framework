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

		public static FieldValidationDefinition AsDefinition(this IOperationResult<FieldIdentifier, bool> @this,
		                                                     string errorText, string loadingText = "Loading...")
			=> new FieldValidationDefinition(@this, errorText, loadingText);
	}
}