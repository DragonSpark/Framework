using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Composition.Compose;
using DragonSpark.Presentation.Components;
using DragonSpark.Presentation.Components.Activity;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;
using ValidationContext = DragonSpark.Presentation.Components.Forms.Validation.ValidationContext;

namespace DragonSpark.Presentation
{
	public static class Extensions
	{
		public static BuildHostContext WithPresentationConfigurations(this BuildHostContext @this)
			=> Configure.Default.Get(@this);

		public static BuildHostContext WithClientConnectionConfigurations(this BuildHostContext @this)
			=> Connections.Configure.Default.Get(@this);

/**/
		public static CallbackContext<ValidationContext> Callback<T>(this ModelContext context,
		                                                             IValidateValue<T> validate)
			=> context.Callback(validate.Adapt());

		public static CallbackContext<ValidationContext> Callback<T>(this ModelContext _,
		                                                             IValidatingValue<T> validating)
			=> validating.Callback();

		public static CallbackContext<ValidationContext> Callback<T>(this IValidatingValue<T> validating)
			=> new ValidationOperationContext(new ValidationOperation<T>(validating)).DenoteExceptions().Get();

		public static IValidateValue<object> Validator(this IExpression @this)
			=> new RegularExpressionValidator(@this.Get());

		public static BoundedExpression Bounded(this IExpression @this) => new BoundedExpression(@this.Get());

		public static IValidatingValue<T> Adapt<T>(this IValidateValue<T> @this)
			=> new ValidatingValueAdapter<T>(@this);

/**/
		public static CallbackContext Callback<T>(this ModelContext @this, EventCallback<T> callback)
			=> @this.Callback(callback.InvokeAsync);
		public static CallbackContext Callback(this ModelContext @this, EventCallback callback)
			=> @this.Callback(new Func<Task>(callback.InvokeAsync));

		public static CallbackContext Callback(this ModelContext @this, Func<ValueTask> method)
			=> @this.Callback(method.Start().Select(x => x.AsTask()));


		public static CallbackContext Callback(this ModelContext _, Func<Task> method) => new CallbackContext(method);

		public static SubmitCallbackContext Callback(this ModelContext _, Func<EditContext, Task> submit)
			=> new SubmitCallbackContext(submit);

		public static CallbackContext<object> Callback(this ModelContext _, Func<object, Task> method)
			=> new CallbackContext<object>(method);

		public static CallbackContext<T> Callback<T>(this ModelContext _, Func<T, Task> method)
			=> new CallbackContext<T>(method);

		public static EditContextCallbackContext Callback(this ModelContext _, EditContext context)
			=> new EditContextCallbackContext(context);

		public static CallbackContext Callback(this ResultContext<Task> @this) => new CallbackContext(@this);

		/*public static Receiver Receiver(this ModelContext _, Action receiver) => new Receiver(receiver);*/

		public static OperationCallbackContext Bind(this IExceptions @this, Func<Task> method)
			=> Start.A.Callback(method).Handle(@this);

		public static OperationCallbackContext<T> Bind<T>(this IExceptions @this, Func<T, Task> method)
			=> Start.A.Callback(method).Handle(@this);

		public static CallbackContext<object> ToCallback(this EventCallback @this)
			=> Start.A.Callback(new Func<object, Task>(@this.InvokeAsync));

		/**/

		public static Evaluation<T> Evaluation<T>(this VowelContext _) where T : ComponentBase
			=> Compose.Evaluation<T>.Default;

		/**/

		public static bool CanSubmit(this EditContext @this) => @this.IsModified() && @this.IsValid();

		public static bool CanSubmit(this EditContext @this, object receiver)
			=> @this.CanSubmit() && !IsActive.Default.Get(receiver);

		public static bool IsValid(this EditContext @this) => !@this.GetValidationMessages().AsValueEnumerable().Any();

		public static bool IsValid(this EditContext @this, object receiver)
			=> @this.IsValid() && !IsActive.Default.Get(receiver);

		public static void NotifyModelField(this EditContext @this, string field)
			=> @this.NotifyFieldChanged(@this.Field(field));

		public static T GetValue<T>(this FieldIdentifier @this)
			=> @this.FieldName.Contains(".")
				   ? PropertyAccess.GetValue(@this.Model, @this.FieldName).To<T>()
				   : SelectValue<T>.Default.Get(@this);

		public static string Text(this RenderFragment @this) => FragmentText.Default.Get(@this);

		public static Dictionary<string, StringValues> QueryString(this NavigationManager @this)
			=> Presentation.QueryString.Default.Get(@this);

		/**/

		public static IQueryView<T> AsView<T>(this IQueryable<T> @this) => new QueryView<T>(@this);
	}
}