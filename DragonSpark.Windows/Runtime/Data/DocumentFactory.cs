using System;
using System.Xml;
using System.Xml.XPath;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Windows.Runtime.Data
{
	public abstract class DocumentFactory<T> : ParameterizedSourceBase<T, IXPathNavigable>
	{
		readonly Action<XmlDocument, T> load;

		protected DocumentFactory( Action<XmlDocument, T> load )
		{
			this.load = load;
		}

		public override IXPathNavigable Get( T parameter )
		{
			var result = new XmlDocument();
			load( result, parameter );
			return result;
		}
	}
}