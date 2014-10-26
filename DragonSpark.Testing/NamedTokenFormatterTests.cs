using System.Collections.Specialized;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing
{
	[TestClass]
	public class NamedTokenFormatterTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBasicValueReplacesCorrectlyFromObject()
		{
			string result = string.Format( new NamedTokenFormatter(), "My name is {0:Name}.", new { Name = "Bob" } );
			Assert.AreEqual( "My name is Bob.", result );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyBasicValueReplacesCorrectlyFromDictionary()
		{
			var dictionary = new ListDictionary { { "Name", "Harry" } };
			string result = string.Format( new NamedTokenFormatter(), "My name is {0:Name}.", dictionary );
			Assert.AreEqual( "My name is Harry.", result );
		}
	}
}
