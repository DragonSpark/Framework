using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Windows.Markup;

namespace DragonSpark.Windows.Setup
{
	public class InitializeMarkupCommand : SetupCommandBase
	{
		[BuildUp]
		protected override void OnExecute( object parameter ) =>
			Ambient.GetCurrent<ISetup>().With( setup => new AssociatedMonitor( setup ).Item.Initialize() );
	}
}