using DragonSpark.Setup;
using DragonSpark.Windows.Markup;

namespace DragonSpark.Windows.Setup
{
	public class InitializeMarkupCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			MarkupExtensionMonitor.Instance.Initialize();
		}
	}
}