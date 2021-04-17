using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	public class EditorComponent : ChangeAwareComponent
	{
		EditContext _editContext = default!;
		public override bool HasChanges => EditContext.IsModified();

		[CascadingParameter]
		EditContext EditContext
		{
			get => _editContext;
			set
			{
				if (_editContext != value)
				{
					if (_editContext.Account() != null)
					{
						_editContext.OnFieldChanged -= OnChanged;
						InvokeAsync(Session.Unregister(this).AsTask);
					}

					if ((_editContext = value) != null)
					{
						_editContext.OnFieldChanged += OnChanged;
					}
				}
			}
		}

		void OnChanged(object? sender, FieldChangedEventArgs e)
		{
			if (EditContext.IsModified(e.FieldIdentifier))
			{
				InvokeAsync(Session.Register(this).AsTask);
			}
		}

		protected override Task Exit()
		{
			EditContext.MarkAsUnmodified();

			return base.Exit();
		}

		protected override void OnDispose(bool disposing)
		{
			EditContext = null!;
			base.OnDispose(disposing);
		}
	}
}