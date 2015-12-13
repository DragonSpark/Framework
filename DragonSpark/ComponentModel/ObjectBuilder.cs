using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ObjectBuilder : IObjectBuilder
	{
		public static ObjectBuilder Instance { get; } = new ObjectBuilder();

		readonly ConcurrentDictionary<string, WeakReference> cache = new ConcurrentDictionary<string, WeakReference>();
		
		public object BuildUp( object target )
		{
			WeakReference value;
			cache.Where( pair => !pair.Value.IsAlive ).Each( pair => cache.TryRemove( pair.Key, out value ) );

			var result = cache.GetOrAdd( target.GetHashCode().ToString(), s =>
			{
				Build( target );
				return new WeakReference( target );
			} ).Target;
			
			return result;
		}

		static void Build( object target )
		{
			var properties = target.GetType().GetRuntimeProperties();
			properties
				.Where( x => x.IsDecoratedWith<System.ComponentModel.DefaultValueAttribute>() )
				.Select( x =>
				{
					var defaultValue = x.PropertyType.Adapt().GetDefaultValue();
					var current = x.GetValue( target, null );

					// var equalsDefault = current.As<string>().With( string.IsNullOrEmpty, () => Equals( current, defaultValue ) );
					var value = Equals( current, defaultValue ) ? x.FromMetadata<System.ComponentModel.DefaultValueAttribute, object>( y => y.AsTo<DefaultAttribute, object>( z => z.GetValue( target, x ), () => y.Value ) ) : null;
					var result = value.With( y => new { Property = x, Value = y } );
					return result;
				} )
				.NotNull()
				.Each( item => item.Property.SetValue( target, item.Value, null ) );
		}
	}
}