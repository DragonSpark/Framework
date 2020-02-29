﻿using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Forms;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using NetFabric.Hyperlinq;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class OperationMonitor : RadzenComponent, IDisposable, ICommand<FieldValidator>
	{
		readonly IDictionary<FieldIdentifier, List<FieldValidator>> _identifiers;

		EditContext _editContext;

		public OperationMonitor() : this(new Dictionary<FieldIdentifier, List<FieldValidator>>()) {}

		public OperationMonitor(IDictionary<FieldIdentifier, List<FieldValidator>> identifiers)
			=> _identifiers = identifiers;

		[Parameter]
		public RenderFragment ChildContent { get; set; }

		[CascadingParameter, UsedImplicitly]
		EditContext EditContext
		{
			get => _editContext;
			set
			{
				if (_editContext != value)
				{
					if (_editContext != null)
					{
						_editContext.OnFieldChanged           -= Changed;
						_editContext.OnValidationStateChanged -= Changed;
						_identifiers.Clear();
					}

					if ((_editContext = value) != null)
					{
						Debounce(() =>
						         {
							         EditContext.OnFieldChanged           += Changed;
							         EditContext.OnValidationStateChanged += Changed;
						         }, 1000);
					}
				}
			}
		}

		public bool IsValid
		{
			get
			{
				var b = !EditContext.GetValidationMessages()
				                    .AsValueEnumerable()
				                    .Any();
				var all = _identifiers.SelectMany(x => x.Value)
				                      .All(x => x.Valid.GetValueOrDefault(false));
				return b
				       &&
				       all;
			}
		}

		public async ValueTask<bool> Validate()
		{
			foreach (var validator in _identifiers.SelectMany(x => x.Value))
			{
				if (!await validator.Validate())
				{
					return false;
				}
			}

			return true;
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
			foreach (var command in _identifiers.SelectMany(x => x.Value))
			{
				command.Execute();
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

		public void Dispose()
		{
			EditContext = null;
		}
	}
}