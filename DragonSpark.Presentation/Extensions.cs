using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValidationContext = DragonSpark.Presentation.Components.Forms.ValidationContext;

namespace DragonSpark.Presentation
{
	public static class Extensions
	{
		public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
			=> Configure.Default.Get(@this);

/**/
		public static Compose.ValidationContext Validation(this ModelContext _) => Compose.ValidationContext.Default;

		public static CallbackContext<ValidationContext> Callback<T>(this ModelContext _,
		                                                             IFieldValidation<T> validation)
			=> validation.Callback();

		public static CallbackContext<ValidationContext> Callback<T>(this IFieldValidation<T> validation)
			=> new ValidationOperationContext(new ValidationOperation<T>(validation)).DenoteExceptions().Get();

		// TODO: Remove
		public static OperationView<TIn, TOut> AsView<TIn, TOut>(this ISelecting<TIn, TOut> @this)
			=> new OperationView<TIn, TOut>(@this);

/**/

		public static CallbackContext Callback(this ModelContext _, Func<Task> method)
			=> new CallbackContext(method);

		public static SubmitCallbackContext Callback(this ModelContext _, Func<EditContext, Task> submit)
			=> new SubmitCallbackContext(submit);

		public static CallbackContext<T> Callback<T>(this ModelContext _, Func<T, Task> method)
			=> new CallbackContext<T>(method);

		public static OperationCallbackContext Bind(this IExceptions @this, Func<Task> method)
			=> Start.A.Callback(method).Handle(@this);

		public static OperationCallbackContext<T> Bind<T>(this IExceptions @this, Func<T, Task> method)
			=> Start.A.Callback(method).Handle(@this);

		public static CallbackContext Callback(this ResultContext<Task> @this) => new CallbackContext(@this);
/**/

		public static bool CanSubmit(this EditContext @this) => @this.IsModified() && @this.IsValid();

		public static bool IsValid(this EditContext @this) => !@this.GetValidationMessages().AsValueEnumerable().Any();

		public static T GetValue<T>(this FieldIdentifier @this)
			=> @this.FieldName.Contains(".")
				   ? PropertyAccess.GetValue(@this.Model, @this.FieldName).To<T>()
				   : SelectValue<T>.Default.Get(@this);

		public static string Text(this RenderFragment @this) => FragmentText.Default.Get(@this);

		public static Dictionary<string, StringValues> QueryString(this NavigationManager @this)
			=> Presentation.QueryString.Default.Get(@this);
	}
}