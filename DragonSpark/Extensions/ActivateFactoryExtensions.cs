using System;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Extensions
{
	public static class ActivateFactoryExtensions
	{
		public static T CreateUsing<T>( this IFactory<ActivateParameter, T> @this, Type type )
		{
			var result = (T)@this.Create( type );
			return result;
		}
	}
}