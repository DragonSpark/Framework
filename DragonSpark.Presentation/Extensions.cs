using DragonSpark.Application;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Model.Interaction;
using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Compose;
using DragonSpark.Presentation.Model;
using DragonSpark.Presentation.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Action = System.Action;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;
using ValidationContext = DragonSpark.Presentation.Components.Forms.Validation.ValidationContext;

namespace DragonSpark.Presentation;

public static class Extensions
{
	public static BuildHostContext WithPresentationConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

	public static BuildHostContext WithCircuitDiagnostics(this BuildHostContext @this)
		=> @this.Configure(CircuitDiagnosticRegistrations.Default);

	/**/
	public static CallbackComposer<ValidationContext> Callback<T>(this ModelContext context, IValidateValue<T> validate)
		=> context.Callback(validate.Adapt());

	public static CallbackComposer<ValidationContext> Callback<T>(this ModelContext _, IValidatingValue<T> validating)
		=> validating.Callback();

	public static CallbackComposer<ValidationContext> Callback<T>(this IValidationMessage<T> @this)
		=> new ValidationOperationComposer(new ValidationMessageOperation<T>(@this)).DenoteExceptions().Get();

	public static CallbackComposer<ValidationContext> Callback<T>(this IValidatingValue<T> @this)
		=> new ValidationOperationComposer(new ValidationOperation<T>(@this)).DenoteExceptions().Get();

	public static IValidatingValue<string> AllowUnassigned(this IValidatingValue<string> @this)
		=> new AllowUnassignedTextAwareValidatingValue(@this);

	/**/
	public static CallbackComposer Callback(this ModelContext @this, EventCallback callback)
		=> @this.Callback(() => callback.Invoke());

	public static CallbackComposer Callback(this ModelContext @this, Func<ValueTask> method)
		=> @this.Callback(method.Start().Select(x => x.AsTask()));

	public static CallbackComposer Callback(this ModelContext _, Func<Task> method) => new(method);

	public static SubmitCallbackComposer Callback(this ModelContext _, Func<EditContext, Task> submit) => new(submit);
	public static SubmitCallbackComposer<T> Submit<T>(this ModelContext _, Func<SubmitInput<T>, Task> submit) => new(submit);

	public static CallbackComposer<object> Callback(this ModelContext _, Func<object, Task> method) => new(method);

	public static CallbackComposer<T> Callback<T>(this ModelContext _, Func<T, Task> method) => new(method);

	public static CallbackComposer<T> Callback<T>(this ModelContext @this, Action<T> callback)
		=> @this.Callback<T>(Start.A.Command(callback).Operation().Allocate());

	public static CallbackComposer<T> Callback<T>(this ModelContext @this, Action callback)
		=> @this.Callback<T>(Start.A.Command(callback).Accept<T>().Operation().Allocate());

	public static CallbackComposer Callback(this ModelContext @this, ICommand<None> command)
		=> @this.Callback(command.Execute);

	public static CallbackComposer Callback(this ModelContext @this, Action callback)
		=> @this.Callback(Start.A.Command(callback).Operation());

	public static EditContextCallbackComposer Callback(this ModelContext _, EditContext context) => new(context);

	public static CallbackComposer Callback(this ResultComposer<ValueTask> @this) => new(@this.Then().Allocate());
	public static CallbackComposer Callback(this ResultComposer<Task> @this) => new(@this);

	public static CallbackComposer<T> Callback<T>(this TaskComposer<T> @this) => new(@this);

	public static DragonSpark.Compose.Model.Operations.OperationComposer<T> Then<T>(this EventCallback<T> @this)
		=> Start.A.Selection<T>().By.Calling(x => @this.Invoke(x)).Then().Structure();

	/**/

	public static Evaluation<T> Evaluation<T>(this VowelContext _) where T : ComponentBase
		=> Compose.Evaluation<T>.Default;

	/**/

	public static bool CanSubmit(this EditContext @this) => @this.IsModified() && @this.IsValid();

	public static bool CanSubmit(this EditContext @this, object receiver)
		=> @this.CanSubmit() && !IsActive.Default.Get(receiver);

	public static ValueTask<bool> Validating(this EditContext @this) => ValidContext.Default.Get(@this);

	public static bool IsValid(this EditContext @this) => Components.Forms.Validation.IsValid.Default.Get(@this);

	public static bool IsValid(this EditContext @this, object receiver)
		=> @this.IsValid() && !IsActive.Default.Get(receiver);


	public static void NotifyModelField(this EditContext @this, string field)
		=> @this.NotifyFieldChanged(@this.Field(field));

	public static T GetValue<T>(this FieldIdentifier @this)
		=> @this.FieldName.Contains('.')
			   ? (T)PropertyAccess.GetValue(@this.Model, @this.FieldName)
			   : SelectValue<T>.Default.Get(@this);
	/**/
	public static RenderFragment Fragment(this string? @this) => x => x.AddContent(0, @this);
	public static RenderFragment Text<T>(this T @this) where T : notnull => @this.ToString().Fragment();
	public static RenderFragment Fragment<T>(this T @this) => x => x.AddContent(0, @this);
	public static string Text(this RenderFragment @this) => FragmentText.Default.Get(@this);

	public static MarkupString AsMarkdown(this string @this) => MarkdownString.Default.Get(@this);

	/**/

	public static ApplicationProfileContext WithAntiforgery(this ApplicationProfileContext @this)
		=> @this.Append(Security.Registrations.Default);

	/**/

	public static SelectionListingCollection<T> ToSelectionListingCollection<T>(
		this Memory<SelectionListing<T>> @this)
		=> Compose.ToSelectionListingCollection<T>.Default.Get(@this);

	public static SelectionListingCollection<T> ToSelectionListingCollection<T>(
		this IEnumerable<SelectionListing<T>> @this) => new(@this);

	public static SelectionListingCollection<T> ToSelectionListingCollection<T>(
		this IEnumerable<SelectionListing<T>> @this, IEqualityComparer<T> comparer) => new(@this, comparer);

	/**/

	public static InteractionResultHandlerComposer<T> Then<T>(this IOperation<T> @this) where T : IInteractionResult
		=> new(@this);

	public static ActiveContentComposer<T> Then<T>(this IActiveContent<T> @this) => new(@this);

	public static Compose.OperationResultComposer<_, T> Then<_, T>(
		this DragonSpark.Compose.Model.Operations.OperationResultComposer<_, T> @this)
		=> new(@this.Out());

	public static Compose.OperationComposer<T> Then<T>(this Application.Compose.OperationComposer<T> @this)
		=> new(@this.Get());

	/**/

	public static Task Invoke<T>(this EventCallback<T> @this, T? parameter)
		=> @this.HasDelegate ? @this.InvokeAsync(parameter) : Task.CompletedTask;

	public static Task Invoke(this EventCallback @this) => @this.HasDelegate ? @this.InvokeAsync() : Task.CompletedTask;

	public static ConfiguredTaskAwaitable On(this EventCallback @this) => @this.Invoke().On();

	public static ConfiguredTaskAwaitable On<T>(this EventCallback<T> @this, T parameter)
		=> @this.Invoke(parameter).On();

	public static ConfiguredTaskAwaitable Off(this EventCallback @this) => @this.Invoke().Off();

	public static ConfiguredTaskAwaitable Off<T>(this EventCallback<T> @this, T parameter)
		=> @this.Invoke(parameter).Off();

	/**/
	public static ActivityOptions Get(this ITokenHandle @this, string message) => new(message, @this);
}