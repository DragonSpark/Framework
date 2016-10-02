using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;
using System.Configuration;
using System.Linq;

namespace DragonSpark.Windows.Setup
{
	public class ConfigurationSectionFactory<T> : SourceBase<T> where T : ConfigurationSection
	{
		readonly Func<string, object> factory;

		public ConfigurationSectionFactory() : this( ConfigurationManager.GetSection ) {}

		public ConfigurationSectionFactory( Func<string, object> factory )
		{
			this.factory = factory;
		}

		public override T Get()
		{
			var name = typeof(T).Name.SplitCamelCase().First().ToLower();
			var result = factory( name ) as T;
			return result;
		}
	}
}