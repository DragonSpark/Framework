using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;
using DragonSpark.Testing.Framework.Application;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class PartsTests
	{
		[Fact]
		public void Public() => PublicAttributed( PublicParts.Default.Get( GetType().Assembly )  );

		[Theory, AutoData, ApplicationPublicParts]
		public void PublicAttributed( ImmutableArray<Type> types )
		{
			Assert.Single( types );
			Assert.Equal( "DragonSpark.Testing.Parts.PublicClass", types.Single().FullName );
		}

		[Fact]
		public void All() => AllAttributed( AllParts.Default.Get( GetType().Assembly ) );

		[Theory, AutoData, ApplicationParts]
		public void AllAttributed( ImmutableArray<Type> types )
		{
			Assert.Equal( 2, types.Length );
			var names = types.Select( type => type.FullName ).ToArray();
			Assert.Contains( "DragonSpark.Testing.Parts.PublicClass", names );
			Assert.Contains( "DragonSpark.Testing.Parts.NonPublicClass", names );
		}
	}
}