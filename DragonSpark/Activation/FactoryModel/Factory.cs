using DragonSpark.Aspects;
using System;

namespace DragonSpark.Activation.FactoryModel
{
	public static class Factory
	{
		public static T Create<T>() => Create<T>( null );

		public static T Create<T>( object context ) => (T)From( FactoryReflectionSupport.Instance.GetFactoryType( typeof(T) ), context );

		public static object From( [OfFactoryType]Type factoryType, object context = null ) => new FactoryBuiltObjectFactory().Create( new ObjectFactoryParameter( factoryType, context ) );
	}
}