using DragonSpark.Application;
using DragonSpark.Application.Components.Validation;
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
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Compose;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Presentation.Model;
using DragonSpark.Presentation.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Action = System.Action;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;
using ValidationContext = DragonSpark.Presentation.Components.Forms.Validation.ValidationContext;

namespace DragonSpark.Presentation;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public BuildHostContext WithPresentationConfigurations() => Configure.Default.Get(@this);

		public BuildHostContext WithCircuitDiagnostics() => @this.Configure(CircuitDiagnosticRegistrations.Default);
	}

	/**/
	extension(ModelContext context)
	{
		public CallbackComposer<Stop<ValidationContext>> Callback<T>(IValidateValue<T> validate)
			=> context.Callback(validate.Adapt());

		public CallbackComposer<Stop<ValidationContext>> Callback<T>(IValidatingValue<T> validating)
			=> validating.Callback();
	}

	public static CallbackComposer<Stop<ValidationContext>> Callback<T>(this IValidationMessage<T> @this)
		=> new ValidationOperationComposer(new ValidationMessageOperation<T>(@this).AsStop()).DenoteExceptions().Get();

	public static CallbackComposer<Stop<ValidationContext>> Callback<T>(this IValidatingValue<T> @this)
		=> new ValidationOperationComposer(new ValidationOperation<T>(@this)).DenoteExceptions().Get();

	public static IValidatingValue<string> AllowUnassigned(this IValidatingValue<string> @this)
		=> new AllowUnassignedTextAwareValidatingValue(@this);

	/**/
	extension(ModelContext @this)
	{
		public CallbackComposer Callback(EventCallback callback, object? owner = null)
			=> new(owner, () => callback.Invoke());

		public CallbackComposer Callback(Func<ValueTask> method)
			=> @this.Callback(method.Start().Select(x => x.AsTask()));

		public CallbackComposer Callback(Func<Task> method) => new(method);

		public SubmitCallbackComposer Callback(Func<EditContext, Task> submit) => new(submit);

		public SubmitCallbackComposer Callback(Func<EditContext, Task> submit,
											   IStopAware invalid, CancellationToken stop)
			=> @this.Callback(submit, invalid.Then().Bind(stop).Out());

		public SubmitCallbackComposer Callback(Func<EditContext, Task> submit, IOperation invalid)
			=> new(submit, invalid);
		public SubmitCallbackComposer Callback(EventCallback<EditContext> method, object? owner = null)
			=> new(x => method.Invoke(x), owner);

		public SubmitWithCancelCallbackComposer Callback(Func<SubmittingInput, Task> submit) => new(submit);

		public CallbackComposer<object> Callback(Func<object, Task> method) => new(method);

		public CallbackComposer<T> Callback<T>(EventCallback<T> method, object? owner = null) => new(owner, x => method.Invoke(x));
		public CallbackComposer<T> Callback<T>(Func<T, Task> method) => new(method);

		public CallbackComposer<T> Callback<T>(Action<T> callback)
			=> @this.Callback<T>(Start.A.Command(callback).Operation().Allocate());

		public CallbackComposer<T> Callback<T>(Action callback)
			=> @this.Callback<T>(Start.A.Command(callback).Accept<T>().Operation().Allocate());

		public CallbackComposer Callback(ICommand<None> command)
			=> @this.Callback(command.Execute);

		public CallbackComposer Callback(Action callback)
			=> new(callback.Target, Start.A.Command(callback).Operation().Allocate());
	}

	// ReSharper disable once TooManyArguments

	extension(IActivityReceiver @this)
	{
		public object Target(Delegate method) => @this.Target(method.Target ?? @this);

		public object Target(object other) => other;
	}

	public static EditContextCallbackComposer Callback(this ModelContext _, EditContext context) => new(context);

	extension(ResultComposer<ValueTask> @this)
	{
		public CallbackComposer Callback() => new(@this.Then().Allocate());

		public CallbackComposer Callback(object receiver)
			=> new(receiver, @this.Then().Allocate());
	}

	public static CallbackComposer Callback(this ResultComposer<Task> @this) => new(@this);

	public static CallbackComposer<T> Callback<T>(this TaskComposer<T> @this) => new(@this);

	public static DragonSpark.Compose.Model.Operations.OperationComposer<T> Then<T>(this EventCallback<T> @this)
		=> Start.A.Selection<T>().By.Calling(x => @this.Invoke(x)).Then().Structure();

	/**/

	public static Evaluation<T> Evaluation<T>(this VowelContext _) where T : ComponentBase
		=> Compose.Evaluation<T>.Default;

	/**/

	extension(EditContext @this)
	{
		public bool CanSubmit() => @this.IsModified() && @this.IsValid();

		public bool CanSubmit(IActivityReceiver receiver) => @this.CanSubmit() && !receiver.Active;

		public ValueTask<bool> Validating() => ValidContext.Default.Get(@this);

		public bool IsValid() => Components.Forms.Validation.IsValid.Default.Get(@this);

		public bool IsValid(IActivityReceiver receiver) => @this.IsValid() && !receiver.Active;

		public void NotifyModelField(string field)
		{
			@this.NotifyFieldChanged(@this.Field(field));
		}

		public void MarkModified()
		{
			@this.NotifyModelField(string.Empty);
			@this.NotifyValidationStateChanged();
		}
	}

	public static T? GetValue<T>(this FieldIdentifier @this)
		=> @this.FieldName.Contains('.')
			   ? (T?)PropertyAccess.GetValue(@this.Model, @this.FieldName)
			   : SelectValue<T>.Default.Get(@this);

	/**/
	/*public static RenderFragment Fragment(this string? @this) => x => x.AddContent(0, @this);
	public static RenderFragment Text<T>(this T @this) where T : notnull => @this.ToString().Fragment();
	public static RenderFragment Fragment<T>(this T @this) => x => x.AddContent(0, @this);*/
	public static string Text(this RenderFragment @this) => FragmentText.Default.Get(@this);

	public static MarkupString AsMarkdown(this string? @this)
		=> !@this.IsNullOrEmpty() ? MarkdownString.Default.Get(@this.Verify()) : new(@this.OrNone());

	/**/

	public static OptionCollection<T> ToOptionCollection<T>(this Memory<Option<T>> @this)
		=> Compose.ToOptionCollection<T>.Default.Get(@this);

	public static OptionCollection<T> ToOptionCollection<T>(this IEnumerable<Option<T>> @this) => new(@this);

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

	extension(EventCallback @this)
	{
		public Task Invoke() => @this.HasDelegate ? @this.InvokeAsync() : Task.CompletedTask;

		public Task Invoke(object? parameter)
			=> @this.HasDelegate ? @this.InvokeAsync(parameter) : Task.CompletedTask;

		public ConfiguredTaskAwaitable On() => @this.Invoke().On();
	}

	public static ConfiguredTaskAwaitable On(this EventCallback<CancellationToken> @this, CancellationToken parameter)
	{
		parameter.ThrowIfCancellationRequested();
		return @this.Invoke(parameter).On();
	}

	public static ConfiguredTaskAwaitable On<T>(this EventCallback<T> @this, T parameter)
		=> @this.Invoke(parameter).On();

	public static ConfiguredTaskAwaitable Off(this EventCallback @this) => @this.Invoke().Off();

	public static ConfiguredTaskAwaitable Off<T>(this EventCallback<T> @this, T parameter)
		=> @this.Invoke(parameter).Off();

	/**/
	// ReSharper disable once TooManyArguments
	public static CancelAwareActivityOptions Get(this IStopHandle @this, string message, IOperation? canceled = null,
												 bool RedrawOnFinish = true)
		=> new(message, @this, RedrawOnFinish: RedrawOnFinish, Canceled: canceled);
	
	/**/
	public static bool IsConnected(this IResult<RenderState> @this)
		=> @this.Get() is RenderState.Connected or RenderState.Established;
	
	/**/
	extension(ResourceAssetCollection @this)
	{
		public string Path<T>() => @this.Path(typeof(T));

		public string Path(Type parameter) => @this[ModulePath.Default.Get(parameter)];
	}

	public static IActivityReceiver? NullIfEmpty(this IActivityReceiver? @this)
		=> @this == EmptyActivityReceiver.Default ? null : @this;
}