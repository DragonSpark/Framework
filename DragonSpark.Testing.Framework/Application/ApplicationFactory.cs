using DragonSpark.Commands;
using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationFactory : ConfigurableParameterizedSource<MethodBase, IApplication>
	{
		public static ApplicationFactory Default { get; } = new ApplicationFactory();
		ApplicationFactory() : base( DefaultImplementation.Implementation.Get ) {}

		sealed class DefaultImplementation : ConfiguringFactory<MethodBase, IApplication>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( _ => new Application( ApplicationCommandSource.Default.ToArray() ), ApplicationInitializer.Default.Execute ) {}
		}
	}
}