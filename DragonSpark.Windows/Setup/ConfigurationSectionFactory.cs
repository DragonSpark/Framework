using System.Configuration;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Setup
{
	public class ConfigurationSectionFactory<T> : FactoryBase<T> where T : ConfigurationSection
	{
		readonly ConfigurationFactory factory;

		public ConfigurationSectionFactory() : this( ConfigurationFactory.Instance )
		{}

		public ConfigurationSectionFactory( [Required]ConfigurationFactory factory )
		{
			this.factory = factory;
		}

		protected override T CreateItem()
		{
			var name = typeof(T).Name.SplitCamelCase().First().ToLower();
			var resolver = factory.Create();
			var result = resolver( name ) as T;
			return result;
		}
	}
}