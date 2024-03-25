using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

public class ConfirmComponent : ChangeAwareComponent
{
	bool _active = true;

	public override bool HasChanges => _active;

	protected override Task Exit()
	{
		_active = false;

		return base.Exit();
	}
}