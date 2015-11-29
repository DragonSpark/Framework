using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;

namespace DragonSpark.Runtime.Values
{
	public static class AmbientValues
	{
		public static void Initialize( IAmbientValueRepository repository )
		{
			Repository = repository;
		}	static IAmbientValueRepository Repository { get; set; } = AmbientValueRepository.Instance;

		public static T Get<T>()
		{
			return Get<T>( null );
		}

		public static T Get<T>( object context )
		{
			var got = Get( typeof(T), context );
			var result = got != null ? (T)got : default(T);
			return result;
		}

		public static object Get( Type type, object context )
		{
			var result = Repository.Locked( repository => repository.Get( new AmbientRequest( type, context ) ) );
			return result;
		}

		public static void RegisterFor<TRequest>( TRequest instance, object context )
		{
			Register( new AmbientKey<TRequest>( new EqualitySpecification( context ) ), instance );
		}

		public static void Register( IAmbientKey key, object instance )
		{
			Repository.Locked( repository => repository.Add( key, instance ) );
		}

		public static void Remove( object context )
		{
			Repository.Locked( repository => repository.Remove( context ) );
		}

		/*public static IDisposable CreateContext( IAmbientKey key, object item )
		{
			var result = new AmbientContext( key, item );
			return result;
		}*/
	}
}