using System;
using System.Reflection;
using System.Runtime.InteropServices;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.ComponentModel
{
	public sealed class ServicesValueProviderConverter : ParameterizedSourceBase<PropertyInfo, Type>
	{
		readonly Func<PropertyInfo, Type> type;

		public ServicesValueProviderConverter( [Optional] Type activatedType ) : this( p => activatedType ?? p.PropertyType ) {}

		public ServicesValueProviderConverter( Func<PropertyInfo, Type> type )
		{
			this.type = type;
		}

		public override Type Get( PropertyInfo parameter ) => type( parameter );
	}
}