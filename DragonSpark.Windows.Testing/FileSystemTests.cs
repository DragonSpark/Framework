using System;
using System.Globalization;
using Xunit;

namespace DragonSpark.Windows.Testing
{
	public class FileSystemTests
	{
		[Fact]
		public void TestMethod1()
		{
			DateTimeOffset result;
			var parsed = DateTimeOffset.TryParseExact( "2015-11-13--18-46-49", Windows.FileSystem.Defaults.ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out result );
			Assert.True( parsed );
		}
	}
}
