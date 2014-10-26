using DragonSpark.Activation;
using DragonSpark.Serialization;
using System;
using System.IO;

namespace DragonSpark.Server.IO
{
	public abstract class SerializationFactory : IFactory
	{
		readonly IStreamResolver resolver;

		protected SerializationFactory( IStreamResolver resolver )
		{
			this.resolver = resolver;
		}

		public IStreamResolver Resolver
		{
			get { return resolver; }
		}

		protected virtual object Create( Type resultType, object parameter )
		{
			var stream = Resolver.ResolveStream();
			if ( stream != null )
			{
				var result = Deserialize( resultType, stream );
				return result;
			}
			throw new InvalidOperationException( string.Format( "Invalid stream resolved by '{0}'.", GetType().Name ) );
		}

		object IFactory.Create( Type resultType, object parameter )
		{
			var result = Create( resultType, parameter );
			return result;
		}

		protected abstract object Deserialize( Type targetType, Stream stream );
	}
}