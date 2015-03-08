using System.Collections;
using System.Diagnostics.Contracts;
using System.Dynamic;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Runtime
{
	public static class ValueResolver
	{
/*
		readonly object source;
		readonly string name;

		public ValueResolver( object source, string name )
		{
			Contract.Requires( source != null );
			Contract.Requires( !string.IsNullOrEmpty( name ) );
			this.source = source;
			this.name = name;
		}
*/

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( source != null );
			Contract.Invariant( !string.IsNullOrEmpty( name ) );
		}*/

/*
		public static ValueResolver Instance
		{
			get { return InstanceField; }
		}	static readonly ValueResolver InstanceField = new ValueResolver();
*/

		public static object Resolve( object source, string name )
		{
			var dictionary = source as IDictionary;
			if ( dictionary != null && dictionary.Contains( name ) )
			{
				var value = dictionary[ name ];
				return value;
			}
			/*var propertyInfo = source.GetType().GetProperty( name );
			if ( propertyInfo != null )
			{
				var result = propertyInfo.GetValue( source, null );
				return result;
			}*/

			var model = source as IDynamicMetaObjectProvider;
			if ( model != null )
			{
				var resolve = model.GetValue( name );
				return resolve;
			}

			var result = source.EvaluateValue( name );
			return result;
		}
	}
}