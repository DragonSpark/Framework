using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing
{
	public class ConfirmComponent : ChangeAwareComponent
	{
		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await Session.Register(this);
		}

		bool Active { get; set; } = true;

		public override bool HasChanges => Active;

		protected override Task Exit()
		{
			Active = false;

			return base.Exit();
		}

	}
}