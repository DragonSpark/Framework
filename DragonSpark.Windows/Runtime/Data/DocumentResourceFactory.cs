using System;

namespace DragonSpark.Windows.Runtime.Data
{
	public sealed class DocumentResourceFactory : DocumentFactory<Uri>
	{
		public static DocumentResourceFactory Default { get; } = new DocumentResourceFactory();
		DocumentResourceFactory() : base( ( document, data ) => document.Load( data.ToString() ) ) {}
	}
}