using System;
using System.Data;
using System.IO;
using System.Linq;
using DragonSpark.Io;
using DragonSpark.Testing.Io;
using DragonSpark.Testing.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Io
{
	/// <summary>
	///This is a test class for JetFileReaderTests and is intended
	///to contain all JetFileReaderTests Unit Tests
	///</summary>
	[TestClass]
	public class JetFileReaderTests
	{
		[TestMethod]
		public void CreateDataSetTest()
		{
			using ( var workspace = Workspace.Create() )
			{
				var path = string.Format( "{0}.txt", Guid.NewGuid() );
				workspace.Write( path, Properties.Resources.CountryInfo );

				var options = new JetReaderOptions( Path.Combine( workspace.Directory.FullName, path ), TextDelimiter.TabDelimited, true ); 
				var target = new JetFileReader( options );
				var table = target.Create();
				Assert.AreEqual( 247, table.Rows.Count );

				var rows = table.Rows.Cast<DataRow>();
				var first = rows.First();
				Assert.AreEqual( first[ "ISO" ], "AD" );

				var last = rows.Last();
				Assert.AreEqual( last[ "ISO" ], "CS" );

				var found = table.Rows.Find( "US" );
				Assert.IsNotNull( found );
				Assert.AreEqual( "United States", found[ "Country" ] );
			}
		}
	}
}