using DragonSpark.Application.Setup;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using Serilog;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class ConstructorSelectorTests
	{
		[Fact]
		public void Basic()
		{
			var parts = this.Adapt().WithNested().Append( typeof(Protected) ).AsApplicationParts();
			var builder = ConventionBuilderFactory.Default.Get();
			var container = new ContainerConfiguration().WithParts( parts.AsEnumerable(), builder ).WithProvider( ServicesExportDescriptorProvider.Default ).CreateContainer();
			new EnableServicesCommand().Execute();
			var dependency = container.GetExport<Dependency>();
			Assert.NotNull( dependency );

			var export = container.GetExport<IPrimary>();
			var primary = Assert.IsType<Primary>( export );
			Assert.Equal( 2, primary.Selected.GetParameters().Length );

			var exported = container.GetExport<Exported>();
			Assert.Equal( 3, exported.Selected.GetParameters().Length );

			var external = container.GetExport<ExternalDependencyExport>();
			Assert.Equal( 3, external.Selected.GetParameters().Length );

			Assert.Throws<CompositionFailedException>( () => container.GetExport<Protected>() );
		}

		public interface IPrimary {}

		public class Primary : IPrimary
		{
			[UsedImplicitly]
			public Primary( IDependency dependency, IAnotherDependency anotherDependency )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			[UsedImplicitly]
			public Primary( IAnotherDependency dependency, IAnotherDependency anotherDependency, NotKnown notKnown )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			public MethodBase Selected { get; }
		}

		[Export]
		class Exported
		{
			[UsedImplicitly]
			public Exported( Dependency dependency, AnotherDependency anotherDependency )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			[UsedImplicitly]
			public Exported( Dependency dependency, AnotherDependency anotherDependency, IAnotherDependencyAgain again )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			public MethodBase Selected { get; }
		}


		[Export]
		class ExternalDependencyExport
		{
			[UsedImplicitly]
			public ExternalDependencyExport( Dependency dependency, AnotherDependency anotherDependency )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			[UsedImplicitly]
			public ExternalDependencyExport( Dependency dependency, AnotherDependency anotherDependency, ILogger logger )
			{
				Selected = MethodBase.GetCurrentMethod();
			}

			public MethodBase Selected { get; }
		}

		[Export]
		class Protected
		{
			[UsedImplicitly]
			protected Protected( Dependency dependency, AnotherDependency anotherDependency ) {}
		}

		public interface IDependency {}
		public class Dependency : IDependency {}

		public interface IAnotherDependency {}
		public class AnotherDependency : IAnotherDependency {}

		interface IAnotherDependencyAgain {}
		[UsedImplicitly]
		public class AnotherDependencyAgain : IAnotherDependencyAgain {}

		public class NotKnown
		{
			NotKnown() {}
		}
	}
}
