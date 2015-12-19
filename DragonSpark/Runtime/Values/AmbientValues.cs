using DragonSpark.Runtime.Specifications;
using System;

namespace DragonSpark.Runtime.Values
{
	public static class AmbientValues
	{
		static AmbientValues()
		{
			Initialize( AmbientValueRepository.Instance );
		}

		public static void Initialize( IAmbientValueRepository repository )
		{
			Repository = repository;
		}	static IAmbientValueRepository Repository { get; set; }

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
			var result = Repository.Get( new AmbientRequest( type, context ) );
			return result;
		}

		public static void RegisterFor<TRequest>( TRequest instance, object context )
		{
			Register( new AmbientKey<TRequest>( new EqualityContextAwareSpecification( context ) ), instance );
		}

		public static void Register( IAmbientKey key, object instance )
		{
			Repository.Add( key, instance );
		}

		public static void Remove( object context )
		{
			Repository.Remove( context );
		}

		/*public static IDisposable CreateContext( IAmbientKey key, object item )
		{
			var result = new AmbientContext( key, item );
			return result;
		}*/
	}
}