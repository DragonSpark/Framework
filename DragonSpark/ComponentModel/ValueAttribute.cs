using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using System;

namespace DragonSpark.ComponentModel
{
	public class ValueAttribute : ActivateAttribute
	{
		public ValueAttribute( Type activatedType, string name = null ) : base( () => new ValueDefaultProvider( activatedType, name ) )
		{}
	}

	public class ValueDefaultProvider : ActivatedValueProvider
	{
		public ValueDefaultProvider( Type activatedType, string name ) : base( activatedType, name )
		{}

		/*public ValueDefaultProvider( IActivator activator, Type activatedType, string name ) : base( activator, activatedType, name )
		{}*/

		protected override object Activate( Parameter parameter )
		{
			var item = base.Activate( parameter );
			var result = item.AsTo<IValue, object>( value => value.Item );
			return result;
		}
	}
}