using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Presentation.Components
{
	public sealed class OperationMonitor : RadzenComponent, IDisposable, ICommand<FieldValidator>
	{
		readonly IDictionary<FieldIdentifier, List<FieldValidator>> _identifiers;

		public OperationMonitor() : this(new Dictionary<FieldIdentifier, List<FieldValidator>>()) {}

		public OperationMonitor(IDictionary<FieldIdentifier, List<FieldValidator>> identifiers)
			=> _identifiers = identifiers;

		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[CascadingParameter]
		public EditContext EditContext { get; [UsedImplicitly] set; }

		public bool IsValid
		{
			get
			{
				var result = !EditContext.GetValidationMessages()
				                         .AsValueEnumerable()
				                         .Any()
				             &&
				             _identifiers.SelectMany(x => x.Value)
				                         .All(x => x.Valid.HasValue && x.Valid.Value);
				return result;
			}
		}

		protected override void OnInitialized()
		{
			Debounce(() =>
			         {
				         EditContext.OnFieldChanged           += Changed;
				         EditContext.OnValidationStateChanged += Changed;
			         });
		}

		public void Refresh()
		{
			EditContext.NotifyValidationStateChanged();
		}

		void Changed(object sender, FieldChangedEventArgs e)
		{
			var list = List(e.FieldIdentifier);

			foreach (var validator in list)
			{
				validator.Reset();
			}

			if (!EditContext.GetValidationMessages(e.FieldIdentifier).Any())
			{
				foreach (var validator in list)
				{
					validator.Start();
				}
			}
		}

		void Changed(object sender, ValidationStateChangedEventArgs e)
		{
			foreach (var validator in _identifiers.SelectMany(x => x.Value))
			{
				validator.Refresh();
			}
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<CascadingValue<OperationMonitor>>(0);
			builder.AddAttribute(1, "IsFixed", true);
			builder.AddAttribute(2, "Value", this);
			builder.AddAttribute(3, "ChildContent", ChildContent);
			builder.CloseComponent();
		}

		public void Dispose()
		{
			if (EditContext != null)
			{
				EditContext.OnFieldChanged           -= Changed;
				EditContext.OnValidationStateChanged -= Changed;
			}
		}

		public void Execute(FieldValidator parameter)
		{
			List(parameter.Identifier).Add(parameter);
		}

		List<FieldValidator> List(FieldIdentifier key)
		{
			if (_identifiers.ContainsKey(key))
			{
				return _identifiers[key];
			}

			var result = new List<FieldValidator>();
			_identifiers[key] = result;
			return result;
		}
	}
}