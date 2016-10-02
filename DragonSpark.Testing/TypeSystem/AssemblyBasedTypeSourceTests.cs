using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	[ContainingTypeAndNested]
	public class AssemblyBasedTypeSourceTests
	{
		[Theory, AutoData]
		[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
		public void Testing( ImmutableArray<Assembly> sut )
		{
			Assert.Equal( AssemblySource.Result, sut );
		}

		[Export]
		class AssemblySource : AssemblyBasedTypeSource
		{
			readonly internal static IEnumerable<Assembly> Result = typeof(AssemblySource).Assembly.Yield();

			public AssemblySource() : base( Result ) {}
		}
	}
}