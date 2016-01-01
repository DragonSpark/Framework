using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Windows;

namespace DragonSpark.Testing.Framework.Setup
{
	public abstract class ResourceConfigurationFactory : FileConfigurationFactory
	{
		protected ResourceConfigurationFactory( Type type ) : base( $"Resources/{type.Name.SplitCamelCase().First()}.config" )
		{ }
	}
}