using DragonSpark.Activation.FactoryModel;
using System;

namespace DragonSpark.Extensions
{
	public static class ActivateFactoryExtensions
	{
		public static T Create<T>( this IFactory<ActivateParameter, T> @this ) => @this.CreateUsing( typeof(T) );

		public static T CreateUsing<T>( this IFactory<ActivateParameter, T> @this, Type type )
		{
			var @using = (T)@this.Create( type );
			return @using;
		}
	}
}