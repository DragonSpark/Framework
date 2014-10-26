﻿using DragonSpark.Activation;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceLocationExtensions
	{
		public static void Register<TFrom, TTo>( this IServiceLocator target )
		{
			Register( target, typeof(TFrom), typeof(TTo) );
		}

		public static void Register( this IServiceLocator target, Type from, Type to )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( from, to ) );
		}

		public static void Register( this IServiceLocator target, Type type, object instance )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( type, instance ) );
		}

		public static void RegisterFactory( this IServiceLocator target, Type type, Func<object> factory )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.RegisterFactory( type, factory ) );
		}

		public static void Register<TService>( this IServiceLocator target, TService instance )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( typeof(TService), instance ) );
		}

		public static TItem TryGetInstance<TItem>( this IServiceLocator target, string name = null )
		{
			var result = target.TryGetInstance( typeof(TItem), name ).To<TItem>();
			return result;
		}

		public static object TryGetInstance( this IServiceLocator target, Type type, string name = null )
		{
			try
			{
				var result = target.GetInstance( type, name );
				return result;
			}
			catch ( ActivationException )
			{}
			return null;
		}
	}
}
