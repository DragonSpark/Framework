using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Objects.FileSystem;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class PartsTests
	{
		[Fact]
		public void Public()
		{
			new InitializePartsCommand().Execute();
			PublicAttributed( PublicParts.Default.Get( GetType().Assembly ) );
		}

		[Theory, AutoData, InitializePartsCommand.Public]
		public void PublicAttributed( ImmutableArray<Type> types )
		{
			var type = Assert.Single( types );
			Assert.Equal( "DragonSpark.Testing.Parts.PublicClass", type.FullName );
		}

		[Fact]
		public void All()
		{
			new InitializePartsCommand().Execute();
			AllAttributed( AllParts.Default.Get( GetType().Assembly ) );
		}

		[Theory, AutoData, InitializePartsCommand.All]
		public void AllAttributed( ImmutableArray<Type> types )
		{
			Assert.Equal( 2, types.Length );
			var names = types.Select( type => type.FullName ).ToArray();
			Assert.Contains( "DragonSpark.Testing.Parts.PublicClass", names );
			Assert.Contains( "DragonSpark.Testing.Parts.NonPublicClass", names );
		}
	}
}