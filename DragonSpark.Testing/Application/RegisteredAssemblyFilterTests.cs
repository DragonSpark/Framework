using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
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
	public class RegisteredAssemblyFilterTests
	{
		[Theory, Framework.Application.AutoData]
		public void Basic( Mock<ITypeSource> provider, RegisteredAssemblyFilter sut )
		{
			provider.Setup( p => p.Get() ).Returns( () => new[] { typeof(AutoDataAttribute), typeof(Framework.Application.AutoDataAttribute) }.ToImmutableArray() );

			var enumerable = provider.Object.Get().AsEnumerable().Assemblies().Fixed();
			var assemblies = sut.Get( enumerable );
			
			provider.Verify( assemblyProvider => assemblyProvider.Get() );
			Assert.NotEqual( assemblies, enumerable );
		}

		[Theory, Framework.Application.AutoData]
		public void Source( [Service]ITypeSource sut )
		{
			Assert.NotNull( sut );
			Assert.Same( AssemblyTypeSource.Default, sut );
		}

		sealed class AssemblyTypeSource : AssemblyBasedTypeSource
		{
			[Export( typeof(ITypeSource) )]
			public static AssemblyTypeSource Default { get; } = new AssemblyTypeSource();
			AssemblyTypeSource() : base( typeof(AssemblyTypeSource) ) {}
		}
	}
}