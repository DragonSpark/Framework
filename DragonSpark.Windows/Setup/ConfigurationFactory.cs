using System;
using System.Configuration;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Windows.Setup
{
	public class ConfigurationFactory : FactoryBase<Func<string, object>>
	{
		public static ConfigurationFactory Instance { get; } = new ConfigurationFactory();

		protected override Func<string, object> CreateItem()
		{
			return ConfigurationManager.GetSection;
		}
	}
}