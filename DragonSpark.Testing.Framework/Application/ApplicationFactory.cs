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
		ApplicationFactory() : base( Implementation.DefaultImplementation.Get ) {}

		sealed class Implementation : ConfiguringFactory<MethodBase, IApplication>
		{
			public static Implementation DefaultImplementation { get; } = new Implementation();
			Implementation() : base( _ => new Application( ApplicationCommandSource.Default.ToArray() ), ApplicationInitializer.Default.Execute ) {}
		}
	}
}