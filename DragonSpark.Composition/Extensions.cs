using LightInject;
using System;

namespace DragonSpark.Composition
{
	static class Extensions
	{
		public static IServiceRegistry RegisterDefinition<T>(this IServiceRegistry @this)
		{
			var to = typeof(T).GetGenericTypeDefinition();
			return @this.Register(to)
			            .RegisterDependencies(to);
		}

		/*public static IServiceRepository RegisterSingleton<T>(this IServiceRepository @this)
		{
			var to = typeof(T).GetGenericTypeDefinition();
			return @this.Register(to)
			            .RegisterDependencies(to);
		}*/

		public static IServiceRegistry RegisterDefinition<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
		{
			var to = typeof(TTo).GetGenericTypeDefinition();
			return @this.Register(to)
			            .Register(typeof(TFrom).GetGenericTypeDefinition(), to)
			            .RegisterDependencies(to);
		}

		public static IServiceRegistry DecorateWithDependencies<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
			=> @this.Decorate<TFrom, TTo>()
			        .RegisterDependencies(typeof(TTo));

		public static IServiceRegistry DecorateDefinition<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
		{
			var to = typeof(TTo).GetGenericTypeDefinition();
			return @this.Register(to)
			            .Decorate(typeof(TFrom).GetGenericTypeDefinition(), to)
			            .RegisterDependencies(to);
		}

		public static IServiceRegistry RegisterWithDependencies<T>(this IServiceRegistry @this)
			=> @this.Register<T>()
			        .RegisterDependencies(typeof(T));

		public static IServiceRegistry RegisterWithDependencies<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
			=> @this.Register<TFrom, TTo>()
			        .RegisterDependencies(typeof(TTo));

		public static IServiceRegistry RegisterDependencies(this IServiceRegistry @this, Type type)
			=> new RegisterDependencies(type).Get(@this);
	}
}
