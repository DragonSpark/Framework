using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	public static class Extensions
	{
		public static ValidationContext Validation(this ModelContext _) => ValidationContext.Default;

		public static OperationView<TIn, TOut> AsView<TIn, TOut>(this IOperationResult<TIn, TOut> @this)
			=> new OperationView<TIn, TOut>(@this);

		public static OperationView<T> AsView<T>(this IOperationResult<T> @this) => new OperationView<T>(@this);

/**/

		public static CallbackContext Callback(this ModelContext _, Func<Task> method)
			=> new CallbackContext(method);

		public static CallbackContext<T> Callback<T>(this ModelContext _, Func<T, Task> method)
			=> new CallbackContext<T>(method);

		public static OperationCallbackContext Bind(this IExceptions @this, Func<Task> method)
			=> Start.A.Callback(method).Handle(@this);

		public static OperationCallbackContext<T> Bind<T>(this IExceptions @this, Func<T, Task> method)
			=> Start.A.Callback(method).Handle(@this);
/**/

		public static T GetValue<T>(this FieldIdentifier @this) => SelectValue<T>.Default.Get(@this);

		public static string Text(this RenderFragment @this) => FragmentText.Default.Get(@this);
	}
}