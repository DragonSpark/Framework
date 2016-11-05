using DragonSpark.Application;
using DragonSpark.Commands;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationFactory : ParameterizedSingletonScope<MethodBase, IApplication>
	{
		public static ApplicationFactory Default { get; } = new ApplicationFactory();
		ApplicationFactory() : base( DefaultImplementation.Implementation.Get ) {}

		sealed class DefaultImplementation : ConfiguringFactory<MethodBase, IApplication>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : base( _ => new Application( ApplicationCommandSource.Default.ToArray() ), ApplicationInitializer.Default.Execute, Assign.Instance.Execute ) {}

			sealed class Assign : AssignScopeCommand<IApplication>
			{
				public static Assign Instance { get; } = new Assign();
				Assign() : base( CurrentApplication.Default ) {}
			}
		}
	}
}