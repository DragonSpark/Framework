using DragonSpark.Application;
using DragonSpark.Diagnostics;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework.FileSystem;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Sources.Scopes
{
	public class ScopesTests
	{
		[Fact]
		public void Coverage()
		{
			var info = FileElement.Empty();
			info.Assign( (byte)2, (byte)3, (byte)4 );
			info.Assign( new byte[]{ 2, 3, 4 }.AsEnumerable() );

			Assert.NotNull( SourceTypes.Default.ToSingleton() );
			Assert.NotNull( Logger.Default.ToExecutionScope() );
			Assert.NotNull( Clock.Default.ToScope() );
		}
	}
}