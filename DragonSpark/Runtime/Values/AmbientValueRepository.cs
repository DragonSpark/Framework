using DragonSpark.Extensions;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime.Values
{
	[ReaderWriterSynchronized]
	public class AmbientValueRepository : IAmbientValueRepository
	{
		public static AmbientValueRepository Instance { get; } = new AmbientValueRepository();

		[Reference]
		readonly IDictionary<IAmbientKey, object> items = new Dictionary<IAmbientKey, object>();

		[Writer]
		public void Add( IAmbientKey key, object instance )
		{
			Remove( key ); // TODO: Handle this a little better.
			items.Add( key, instance );
		}

		[Reader]
		public object Get( IAmbientRequest request )
		{
			var result = items.Keys.FirstOrDefault( key => key.Handles( request ) ).With( key => items[ key ] );
			return result;
		}

		[Writer]
		public void Remove( object context )
		{
			var type = context.AsTo<AmbientKey, Type>( key => key.TargetType ) ?? typeof(object); // TODO: Need to fix how repository behaves.  Very brittle ATM.
			var request = new AmbientRequest( type, context );
			var remove = items.Keys.Where( key => key == context || items[key] == context || key.Handles( request ) ).ToArray();
			remove.Each( value => items.Remove( value ) );
		}
	}
}