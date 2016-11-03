using DragonSpark.Activation.Location;
using DragonSpark.Commands;
using DragonSpark.Sources.Scopes;
using System.Collections.Generic;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	public class ServiceProviderConfigurations : SuppliedCommandSource
	{
		protected override IEnumerable<ICommand> Yield()
		{
			yield return GlobalServiceProvider.Default.ToCommand( ActivatorFactory.Default.ToSingleton() );
		}
	}
}