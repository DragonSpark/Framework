using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	public interface IAmbientValueRepository
	{
		void Add( IAmbientKey key, object instance );

		object Get( IAmbientRequest request );

		void Remove( object context );
	}

	public class AmbientValueRepository : IAmbientValueRepository
	{
		public static AmbientValueRepository Instance { get; } = new AmbientValueRepository();

		readonly IDictionary<IAmbientKey, object> items = new Dictionary<IAmbientKey, object>();

		public void Add( IAmbientKey key, object instance )
		{
			Remove( key ); // TODO: Handle this a little better.
			items.Add( key, instance );
		}

		public object Get( IAmbientRequest request )
		{
			var result = items.Keys.FirstOrDefault( key => key.Handles( request ) ).Transform( key => items[ key ] );
			return result;
		}

		public void Remove( object context )
		{
			var type = context.AsTo<AmbientKey, Type>( key => key.TargetType ) ?? typeof(object); // TODO: Need to fix how repository behaves.  Very brittle ATM.
			var request = new AmbientRequest( type, context );
			var remove = items.Keys.Where( key => key == context || items[key] == context || key.Handles( request ) ).ToArray();
			remove.Apply( value => items.Remove( value ) );
		}
	}
}