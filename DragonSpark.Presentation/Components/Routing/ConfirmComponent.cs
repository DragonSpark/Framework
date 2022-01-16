using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

public class ConfirmComponent : ChangeAwareComponent
{
	bool Active { get; set; } = true;

	public override bool HasChanges => Active;

	protected override Task Exit()
	{
		Active = false;

		return base.Exit();
	}

}