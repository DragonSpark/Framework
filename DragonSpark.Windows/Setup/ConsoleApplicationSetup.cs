using DragonSpark.Setup;

namespace DragonSpark.Windows.Setup
{
	public class ConsoleApplicationSetup : Setup<string[]> {}

	/*public class ConfigurationFactory : FactoryBase<Func<string, object>>
	{
		public static ConfigurationFactory Instance { get; } = new ConfigurationFactory();

		protected override Func<string, object> CreateItem() => ;
	}*/
}