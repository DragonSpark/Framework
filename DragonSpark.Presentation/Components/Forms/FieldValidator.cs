using DragonSpark.Model;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public class FieldValidator : RadzenComponent, ICommand, IDisposable
	{
		readonly Func<Task> _validate;

		EditContext            _editContext;
		FieldValidationContext _context;

		public FieldValidator() => _validate = Refresh;

		[Parameter, UsedImplicitly]
		public bool Popup { get; set; }

		[Parameter, UsedImplicitly]
		public IValidationDefinition Definition { get; set; }

		[Parameter, UsedImplicitly]
		public string Component { get; set; }

		public FieldIdentifier Identifier { get; private set; }

		[Inject]
		internal ILogger<FieldValidator> Logger { get; [UsedImplicitly] set; }

		[CascadingParameter, UsedImplicitly]
		EditOperationContext Operations { get; set; }

		FieldValidationContext Context
		{
			get => _context;
			set
			{
				_context?.Execute(Identifier);
				_context = value;
			}
		}

		[CascadingParameter, UsedImplicitly]
		IRadzenForm Form { get; set; }

		[CascadingParameter, UsedImplicitly]
		EditContext EditContext
		{
			get => _editContext;
			set
			{
				if (_editContext != value)
				{
					var register = _editContext != null;
					_editContext = value;
					if (register)
					{
						Register();
					}
				}
			}
		}

		public bool? Valid => Context.Valid;

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Register();
		}

		void Register()
		{
			Context    = new FieldValidationContext(Definition, EditContext);
			Identifier = Form.FindComponent(Component).FieldIdentifier;
			Operations.Execute(this);
		}

		public void Reset()
		{
			Context.Execute(Identifier);
		}

		public void Start()
		{
			InvokeAsync(_validate);
		}

		public void Execute(None _)
		{
			StateHasChanged();
		}

		protected override string GetComponentCssClass()
			=> $"ui-message ui-messages-{(Context.Valid.HasValue ? "error" : "active")} {(Popup ? "ui-message-popup" : string.Empty)}";

		public async ValueTask<bool> Validate()
		{
			if (Valid.GetValueOrDefault(false))
			{
				return true;
			}

			await Context.Get(this).ConfigureAwait(false);

			if (!Valid.HasValue)
			{
				throw new
					InvalidOperationException($"An attempt was made to validate a field '{Component}' but a validation result could not be attained.");
			}

			return Valid.Value;
		}

		async Task Refresh()
		{
			await Context.Get(this);

			EditContext.NotifyValidationStateChanged();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Visible)
			{
				var text = Context.Text;
				if (text != null)
				{
					builder.OpenElement(0, "div");
					builder.AddAttribute(1, "style", Style);
					builder.AddAttribute(2, "class", GetCssClass());
					builder.AddMultipleAttributes(3, Attributes);
					builder.AddContent(4, text);
					builder.CloseElement();
				}
			}
		}

		// ReSharper disable once FlagArgument
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Operations?.Execute(Identifier);
				Context?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}