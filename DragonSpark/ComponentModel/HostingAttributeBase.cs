using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.ComponentModel
{
	public abstract class HostingAttributeBase : Attribute, IParameterizedSource<object, object>
	{
		readonly Func<object, object> inner;

		protected HostingAttributeBase( Func<object, object> inner )
		{
			this.inner = inner;
		}

		public object Get( object parameter ) => inner( parameter );
	}
}