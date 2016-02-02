using DragonSpark.Aspects;
using DragonSpark.Setup;
using DragonSpark.Windows.Markup;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Setup
{
	public class InitializeMarkupCommand : SetupCommandBase
	{
		/*[Singleton]
		public MarkupExtensionMonitor Monitor { get; set; }*/

		[Required]
		public object Context { [return: Required]get; set; }

		[BuildUp]
		protected override void OnExecute( object parameter ) => new AssociatedMonitor( Context ).Item.Initialize();
	}
}