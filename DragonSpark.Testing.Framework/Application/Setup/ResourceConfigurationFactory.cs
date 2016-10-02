using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Windows.Setup;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class ResourceConfigurationFactory : FileConfigurationFactory
	{
		protected ResourceConfigurationFactory( Type type ) : base( $"Resources/{type.Name.SplitCamelCase().First()}.config" )
		{ }
	}
}