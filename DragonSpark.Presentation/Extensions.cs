using DragonSpark.Application;
using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Compose;
using DragonSpark.Presentation.Model;
using DragonSpark.Presentation.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action = System.Action;
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

/**/
		public static CallbackContext<T> Callback<T>(this ModelContext @this, EventCallback<T> callback)
			=> @this.Callback<T>(callback.InvokeAsync);

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

		public static CallbackContext<T> Callback<T>(this ModelContext @this, Action callback)
			=> @this.Callback<T>(Start.A.Command(callback).Accept<T>().Operation().Allocate());

		public static CallbackContext Callback(this ModelContext @this, ICommand<None> command)
			=> @this.Callback(command.Execute);

		public static CallbackContext Callback(this ModelContext @this, Action callback)
			=> @this.Callback(Start.A.Command(callback).Operation());

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

		public static OperationContext<T> Then<T>(this EventCallback<T> @this)
			=> Start.A.Selection<T>().By.Calling(new Func<T, Task>(@this.InvokeAsync)).Then().Structure();

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
			=> Components.Navigation.QueryString.Default.Get(@this);

		/**/

		public static IQueryView<T> AsView<T>(this IQueryable<T> @this, EntityQuery<T> query)
			=> new QueryView<T>(@this, query);

		public static IQueryView<T> AsView<T>(this IQueryable<T> @this, EntityQuery<T> query, string filter)
			=> new QueryView<T>(@this, query, new FilterAwareQueryAlteration<T>(filter));

		/**/

		public static ApplicationProfileContext WithContentSecurity(this ApplicationProfileContext @this)
			=> @this.Then(x => x.AddContentSecurity()).Then(x => x.UseContentSecurity());

		public static BuildHostContext WithContentSecurity(this BuildHostContext @this)
			=> @this.Configure(Registrations.Default);

		public static IServiceCollection AddContentSecurity(this IServiceCollection @this)
			=> Registrations.Default.Parameter(@this);

		public static IApplicationBuilder UseContentSecurity(this IApplicationBuilder @this)
			=> @this.UseMiddleware<ApplyPolicy>();

		/**/

		public static SelectionListingCollection<T> ToSelectionListingCollection<T>(
			this IEnumerable<SelectionListing<T>> @this) => new(@this);

		public static SelectionListingCollection<T> ToSelectionListingCollection<T>(
			this IEnumerable<SelectionListing<T>> @this, IEqualityComparer<T> comparer) => new(@this, comparer);
	}
}