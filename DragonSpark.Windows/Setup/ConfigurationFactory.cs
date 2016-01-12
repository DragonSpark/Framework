using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Setup
{
	public class ConsoleSetupParameter : ApplicationSetupParameter<string[]>
	{
		public ConsoleSetupParameter( IServiceLocator locator, string[] arguments ) : base( locator, arguments ) {}
	}

	/*public class ConfigurationFactory : FactoryBase<Func<string, object>>
	{
		public static ConfigurationFactory Instance { get; } = new ConfigurationFactory();

		protected override Func<string, object> CreateItem() => ;
	}*/
}