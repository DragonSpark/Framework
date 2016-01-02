using System;
using System.Xml;
using System.Xml.XPath;

namespace DragonSpark.Windows.Runtime.Data
{
	

	public static class DataBuilder
	{
		public static IXPathNavigable Create( string data )
		{
			var result = new XmlDocument();
			result.LoadXml( data );
			return result;
		}

		public static IXPathNavigable Create( Uri location )
		{
			var result = new XmlDocument();
			result.Load( location.ToString() );
			return result;
		}
	}
}