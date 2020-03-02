using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Compose;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation
{
	public static class Extensions
	{
		public static ValidationContext Validation(this ModelContext _) => ValidationContext.Default;

		public static OperationView<TIn, TOut> AsView<TIn, TOut>(this IOperationResult<TIn, TOut> @this)
			=> new OperationView<TIn, TOut>(@this);

		public static OperationView<T> AsView<T>(this IOperationResult<T> @this) => new OperationView<T>(@this);

		public static T GetValue<T>(this FieldIdentifier @this) => @this.Model.GetType()
		                                                                .GetProperty(@this.FieldName)
		                                                                .GetValue(@this.Model)
		                                                                .To<T>(); // TODO: Optimize with delegate.
	}
}