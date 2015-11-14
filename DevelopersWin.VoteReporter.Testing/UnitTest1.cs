using System;
using System.Globalization;
using DragonSpark.Windows.Io;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevelopersWin.VoteReporter.Testing
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			DateTimeOffset result;
			var parsed = DateTimeOffset.TryParseExact( "2015-11-13--18-46-49", FileSystem.ValidPathTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out result );
			Assert.IsTrue( parsed );
		}
	}
}
