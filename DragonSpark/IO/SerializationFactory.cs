using System;
using System.Diagnostics.Contracts;
using System.IO;
using DragonSpark.Objects;
using DragonSpark.Serialization;

namespace DragonSpark.Io
{
	public abstract class SerializationFactory : IFactory
	{
    	readonly IStreamResolver resolver;

    	protected SerializationFactory( IStreamResolver resolver )
    	{
			Contract.Requires( resolver != null );
    		this.resolver = resolver;
    	}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( resolver != null );
		}*/

    	public IStreamResolver Resolver
    	{
    		get { return resolver; }
    	}

		protected virtual object Create( Type resultType, object source )
		{
			var stream = Resolver.ResolveStream();
			if ( stream != null )
			{
				var result = Deserialize( resultType, stream );
				return result;
			}
			throw new InvalidOperationException( string.Format( "Invalid stream resolved by '{0}'.", GetType().Name ) );
		}

		object IFactory.Create( Type resultType, object source )
		{
			var result = Create( resultType, source );
			return result;
		}

		protected abstract object Deserialize( Type targetType, Stream stream );
	}
}