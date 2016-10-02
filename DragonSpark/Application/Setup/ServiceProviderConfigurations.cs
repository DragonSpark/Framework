using DragonSpark.Activation.Location;
using DragonSpark.Commands;
using DragonSpark.Sources;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	public class ServiceProviderConfigurations : SuppliedCommandSource
	{
		protected ServiceProviderConfigurations() {}

		protected override IEnumerable<ICommand> Yield()
		{
			yield return GlobalServiceProvider.Default.Configured( ActivatorFactory.Default.ToCachedDelegate() );
		}
	}
}