using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Configuration;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Configurations" )]
	public class UnityServiceLocatorDefinition : InstanceSourceBase<IServiceLocator>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected override IServiceLocator Create()
		{
			var result = new UnityServiceLocator( new Microsoft.Practices.Unity.UnityContainer() );
			Configurations.Apply( x => x.Configure( result.Container ) );
			return result;
		}

		public Collection<IContainerConfigurationCommand> Configurations
		{
			get { return configurations; }
		}	readonly Collection<IContainerConfigurationCommand> configurations = new Collection<IContainerConfigurationCommand>();
	}
}
