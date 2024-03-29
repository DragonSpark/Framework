﻿using DragonSpark.Application;
using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Model.Interaction;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content;
using DragonSpark.Presentation.Components.Content.Sequences;
using DragonSpark.Presentation.Components.Forms;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Compose;
using DragonSpark.Presentation.Model;
using DragonSpark.Presentation.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
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
	public static CallbackContext<ValidationContext> Callback<T>(this ModelContext context, IValidateValue<T> validate)
		=> context.Callback(validate.Adapt());

	public static CallbackContext<ValidationContext> Callback<T>(this ModelContext _, IValidatingValue<T> validating)
		=> validating.Callback();

	public static CallbackContext<ValidationContext> Callback<T>(this IValidationMessage<T> @this)
		=> new ValidationOperationContext(new ValidationMessageOperation<T>(@this)).DenoteExceptions().Get();

	public static CallbackContext<ValidationContext> Callback<T>(this IValidatingValue<T> @this)
		=> new ValidationOperationContext(new ValidationOperation<T>(@this)).DenoteExceptions().Get();

	public static IValidatingValue<string> AllowUnassigned(this IValidatingValue<string> @this)
		=> new AllowUnassignedTextAwareValidatingValue(@this);

	/**/
	public static CallbackContext Callback(this ModelContext @this, EventCallback callback)
		=> @this.Callback(() => callback.InvokeAsync());

	public static CallbackContext Callback(this ModelContext @this, Func<ValueTask> method)
		=> @this.Callback(method.Start().Select(x => x.AsTask()));

	public static CallbackContext Callback(this ModelContext _, Func<Task> method) => new(method);

	public static SubmitCallbackContext Callback(this ModelContext _, Func<EditContext, Task> submit) => new(submit);

	public static CallbackContext<object> Callback(this ModelContext _, Func<object, Task> method) => new(method);

	public static CallbackContext<T> Callback<T>(this ModelContext _, Func<T, Task> method) => new(method);

	public static CallbackContext<T> Callback<T>(this ModelContext @this, Action callback)
		=> @this.Callback<T>(Start.A.Command(callback).Accept<T>().Operation().Allocate());

	public static CallbackContext Callback(this ModelContext @this, ICommand<None> command)
		=> @this.Callback(command.Execute);

	public static CallbackContext Callback(this ModelContext @this, Action callback)
		=> @this.Callback(Start.A.Command(callback).Operation());

	public static EditContextCallbackContext Callback(this ModelContext _, EditContext context) => new(context);

	public static CallbackContext Callback(this ResultContext<Task> @this) => new(@this);

	public static CallbackContext<T> Callback<T>(this TaskSelector<T> @this) => new(@this);

	public static OperationContext<T> Then<T>(this EventCallback<T> @this)
		=> Start.A.Selection<T>().By.Calling(x => @this.InvokeAsync(x)).Then().Structure();

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

	public static MarkupString AsMarkdown(this string @this) => MarkDownify.Default.Get(@this);

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

	public static Compose.OperationResultSelector<_, T> Then<_, T>(
		this DragonSpark.Compose.Model.Operations.OperationResultSelector<_, T> @this)
		=> new(@this.Out());

	public static Compose.OperationComposer<T> Then<T>(this Application.Compose.OperationComposer<T> @this)
		=> new(@this.Get());

/**/

	public static Task Invoke<T>(this EventCallback<T> @this, T parameter)
		=> @this.HasDelegate ? @this.InvokeAsync(parameter) : Task.CompletedTask;

	public static Task Invoke(this EventCallback @this) => @this.HasDelegate ? @this.InvokeAsync() : Task.CompletedTask;

/**/

	public static IPages<T> Aware<T>(this IPages<T> @this, IPageContainer<T> container)
		=> new ContainerAwarePages<T>(container, @this);
}