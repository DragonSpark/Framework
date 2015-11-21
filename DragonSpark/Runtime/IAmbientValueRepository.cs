using DragonSpark.Extensions;
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
			var result = items.Keys.FirstOrDefault( value => value.Handles( request ) ).Transform( key => items[ key ] );
			return result;
		}

		public void Remove( object context )
		{
			var request = new AmbientRequest( typeof(object), context );
			items.Keys.Where( key => key == context || items[key] == context || key.Handles( request ) ).ToArray().Apply( value => items.Remove( value ) );
		}
	}
}