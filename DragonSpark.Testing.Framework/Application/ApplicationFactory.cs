using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationFactory : ParameterizedScopedSingleton<MethodBase, IApplication>
	{
		public static ApplicationFactory Default { get; } = new ApplicationFactory();
		ApplicationFactory() : base( DefaultImplementation.Implementation.Get ) {}

		sealed class DefaultImplementation : ConfiguringFactory<MethodBase, IApplication>
		{
			readonly static Action<MethodBase> Initialize = ApplicationInitializer.Default.Execute;
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( _ => new Application( ApplicationCommandSource.Default.ToArray() ), Initialize ) {}
		}
	}
}