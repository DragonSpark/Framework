using DragonSpark.Sources.Parameterized;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationFactory : ConfiguringFactory<MethodBase, IApplication>
	{
		public static ApplicationFactory Default { get; } = new ApplicationFactory();
		ApplicationFactory() : base( DefaultCreate, Initialize ) {}

		static IApplication DefaultCreate( MethodBase _ ) => new Application( ApplicationCommandSource.Default.Get().ToArray() );

		static void Initialize( MethodBase method ) => ApplicationInitializer.Default.Get().Execute( method );
	}
}