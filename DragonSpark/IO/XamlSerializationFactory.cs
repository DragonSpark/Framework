using System;
using System.IO;
using DragonSpark.Serialization;

namespace DragonSpark.Io
{
	public class XamlSerializationFactory : SerializationFactory
	{
		public XamlSerializationFactory( IStreamResolver resolver ) : base( resolver )
		{}

		protected override object Deserialize( Type targetType, Stream stream )
		{
			var result = XamlSerializationHelper.Load( stream );
			return result;
		}
	}
}