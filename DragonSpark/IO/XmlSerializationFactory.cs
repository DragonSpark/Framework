using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Serialization;
using DragonSpark.Serialization;

namespace DragonSpark.Io
{
	public class XmlSerializationFactory : SerializationFactory
	{
		public XmlSerializationFactory( IStreamResolver resolver ) : base( resolver )
		{
			Contract.Requires( resolver != null );
		}

		protected override object Deserialize( Type targetType, Stream stream )
		{
			if ( stream != null )
			{
				using ( stream )
				{
					var serializer = new XmlSerializer( targetType );
					var result = serializer.Deserialize( stream );
					return result;
				}
			}
			return null;
		}
	}
}