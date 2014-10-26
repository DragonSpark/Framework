using System;
using System.Globalization;
using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing
{
	[TestClass]
	public class TypeConverterValueConverterTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConvertTo()
		{
			var converter = new TypeConverterValueConverter();
			var converted = converter.Convert( "6775", typeof(int), null, CultureInfo.CurrentCulture );
			Assert.AreEqual( 6775, converted );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConvertToDate()
		{
			var converter = new TypeConverterValueConverter();
			const string date = "6/7/76";
			var converted = converter.Convert( date, typeof(DateTime), null, CultureInfo.CurrentCulture );
			Assert.AreEqual( DateTime.Parse( date ), converted );
		}

		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void VerifyConvertFrom()
		{
			var converter = new TypeConverterValueConverter();
			var converted = converter.ConvertBack( 6776, typeof(string), null, CultureInfo.CurrentCulture );
			Assert.AreEqual( "6776", converted );
		}
	}
}