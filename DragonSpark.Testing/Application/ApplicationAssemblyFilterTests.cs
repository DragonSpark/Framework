using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using Moq;
using System.Collections.Immutable;
using System.Composition;
using Xunit;
using AutoDataAttribute = Ploeh.AutoFixture.Xunit2.AutoDataAttribute;

namespace DragonSpark.Testing.Application
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation ), ContainingTypeAndNested]
	public class ApplicationAssemblyFilterTests
	{
		[Theory, Framework.Application.AutoData]
		public void Basic( Mock<ITypeSource> provider, ApplicationAssemblyFilter sut )
		{
			provider.Setup( p => p.Get() ).Returns( () => new[] { typeof(AutoDataAttribute), typeof(Framework.Application.AutoDataAttribute) }.ToImmutableArray() );

			var enumerable = provider.Object.Get().AsEnumerable().Assemblies().Fixed();
			var assemblies = sut.Get( enumerable );
			
			provider.Verify( assemblyProvider => assemblyProvider.Get() );
			Assert.NotEqual( assemblies, enumerable );
		}

		[Theory, Framework.Application.AutoData]
		public void DeclaredProvider( ITypeSource sut )
		{
			Assert.NotNull( sut );
			Assert.Same( DeclaredAssemblyProvider.Default, sut );
		}

		class DeclaredAssemblyProvider : AssemblyBasedTypeSource
		{
			[Export( typeof(ITypeSource) )]
			public static DeclaredAssemblyProvider Default { get; } = new DeclaredAssemblyProvider();
			DeclaredAssemblyProvider() : base( typeof(DeclaredAssemblyProvider) ) {}
		}
	}
}