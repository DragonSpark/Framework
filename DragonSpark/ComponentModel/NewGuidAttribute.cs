using System;

namespace DragonSpark.ComponentModel
{
	public sealed class NewGuidAttribute : DefaultAttribute
	{
		public NewGuidAttribute() : this( Guid.NewGuid() )
		{}

		public NewGuidAttribute( string value ) : base( Guid.Parse( value ) )
		{}

		public NewGuidAttribute( Guid guid ) : base( guid )
		{}
	}
}