using System.Globalization;
using DragonSpark.IoC;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.IoC
{
	[TestClass]
	public class NamedTypeBuildKeyConverterTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedTypeBuildKeyConverterConvertsToKeyCorrectly()
		{
			var name = "testing";
			var converter = new NamedTypeBuildKeyConverter();
			var input = new NamedTypeBuildKey( typeof(string), name ).ToString();
			var result = (NamedTypeBuildKey)converter.ConvertFrom( null, CultureInfo.CurrentCulture, input );
			Assert.AreEqual( typeof(string), result.Type );
			Assert.AreEqual( name, result.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedTypeBuildKeyConverterConvertsFullyQualifiedTypeToKeyCorrectly()
		{
			var name = "testing";
			var converter = new NamedTypeBuildKeyConverter();
			var input = string.Format( "Build Key[{0}, {1}]", typeof(NamedTypeBuildKeyConverter).AssemblyQualifiedName, name );
			var result = (NamedTypeBuildKey)converter.ConvertFrom( null, CultureInfo.CurrentCulture, input );
			Assert.AreEqual( typeof(NamedTypeBuildKeyConverter), result.Type );
			Assert.AreEqual( name, result.Name );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyNamedTypeBuildKeyConverterConvertsToKeyWithNullNameCorrectly()
		{
			var converter = new NamedTypeBuildKeyConverter();
			var input = new NamedTypeBuildKey( typeof(string), null ).ToString();
			var result = (NamedTypeBuildKey)converter.ConvertFrom( null, CultureInfo.CurrentCulture, input );
			Assert.AreEqual( typeof(string), result.Type );
			Assert.IsNull( result.Name );
		}
	}
}