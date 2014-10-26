using System;
using System.Configuration;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public static class Configuration
	{
		public static T Get<T>( this Type @this, string key, T defaultValue = default(T) )
		{
			var result = @this.Get( key, defaultValue, typeof(T) ).To<T>();
			return result;
		}

		public static object Get( this Type @this, string key, object defaultValue = null, Type resultType = null )
		{
			var setting = string.Concat( @this.Transform( x => string.Concat( x.FullName, "::" ) ), key );
			var value = ConfigurationManager.AppSettings.Get( setting ) ?? defaultValue;
			var result = value.Transform( x => x.ConvertTo( resultType ?? defaultValue.Transform( y => y.GetType() ) ?? typeof(string) ) );
			return result;
		}
	}
}