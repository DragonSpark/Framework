using DragonSpark.Activation;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentExecution : AssignedLogical<Tuple<string>>, IExecutionContext
	{
		public static CurrentExecution Instance { get; } = new CurrentExecution();
	}

	// TODO: See if this is fixed with Shadow Copying.
	[Serializable]
	public class MethodContext
	{
		// readonly string method;

		readonly static ConcurrentDictionary<string, Tuple<string>> Cache = new ConcurrentDictionary<string, Tuple<string>>();

		public static Tuple<string> Get( MethodBase method )
		{
			var key = $"{method.DeclaringType}=>{method}";
			var result = Cache.GetOrAdd( key, Tuple.Create );
			return result;
		}

		/*public static implicit operator MethodContext( MethodBase method )
		{
			var result = Get( method );
			return result;
		}

		MethodContext( string method )
		{
			this.method = method;
		}

		protected bool Equals( MethodContext other )
		{
			return string.Equals( method, other.method, StringComparison.OrdinalIgnoreCase );
		}

		public override bool Equals( object obj )
		{
			if ( ReferenceEquals( null, obj ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, obj ) )
			{
				return true;
			}
			if ( obj.GetType() != typeof(MethodContext) )
			{
				return false;
			}
			return Equals( (MethodContext)obj );
		}

		public override int GetHashCode()
		{
			return ( method != null ? StringComparer.OrdinalIgnoreCase.GetHashCode( method ) : 0 );
		}

		public static bool operator ==( MethodContext left, MethodContext right )
		{
			return Equals( left, right );
		}

		public static bool operator !=( MethodContext left, MethodContext right )
		{
			return !Equals( left, right );
		}*/
	}
}