using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System;
using System.Xml;
using System.Xml.XPath;

namespace DragonSpark.Windows.Runtime.Data
{
	public abstract class DocumentFactory<TParameter> : FactoryBase<TParameter, IXPathNavigable>
	{
		readonly Action<XmlDocument, TParameter> load;

		protected DocumentFactory( [Required]Action<XmlDocument, TParameter> load )
		{
			this.load = load;
		}

		protected override IXPathNavigable CreateItem( TParameter parameter )
		{
			var result = new XmlDocument();
			load( result, parameter );
			return result;
		}
	}

	public class DocumentFactory : DocumentFactory<string>
	{
		public static DocumentFactory Instance { get; } = new DocumentFactory();

		public DocumentFactory() : base( ( document, data ) => document.LoadXml( data ) )
		{}
	}

	public class RemoteDocumentFactory : DocumentFactory<Uri>
	{
		public static RemoteDocumentFactory Instance { get; } = new RemoteDocumentFactory();

		public RemoteDocumentFactory() : base( ( document, data ) => document.Load( data.ToString() ) )
		{}
	}
}