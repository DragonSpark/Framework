using System;

namespace DragonSpark.ComponentModel
{
	public sealed class NewGuidAttribute : DefaultAttribute
	{
		public NewGuidAttribute() : this( Guid.NewGuid() )
		{}

		public NewGuidAttribute( string value ) : this( Guid.Parse( value ) )
		{}

		public NewGuidAttribute( Guid value ) : base( value )
		{}
	}
}