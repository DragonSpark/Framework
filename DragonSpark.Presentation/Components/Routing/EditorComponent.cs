using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	public class EditorComponent : ChangeAwareComponent
	{
		public override bool HasChanges => EditContext.IsModified();

		[CascadingParameter]
		EditContext EditContext { get; set; } = default!;

		protected override Task Exit()
		{
			EditContext.MarkAsUnmodified();

			return base.Exit();
		}
	}
}