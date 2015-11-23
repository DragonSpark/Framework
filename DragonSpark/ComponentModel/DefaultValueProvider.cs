using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[LifetimeManager( typeof(TransientLifetimeManager) )]
	public class DefaultValueProvider : IDefaultValueProvider
	{
		public static DefaultValueProvider Instance { get; } = new DefaultValueProvider();

		readonly IList<WeakReference<object>> items = new List<WeakReference<object>>();

		public void Apply( object target )
		{
			items.Locked( list => list.CheckWith( target, o =>
			{
				var runtimeProperties = target.GetType().GetRuntimeProperties();
				runtimeProperties
					.Where( x => x.IsDecoratedWith<System.ComponentModel.DefaultValueAttribute>() )
					.Select( x =>
					{
						var defaultValue = x.PropertyType.GetDefaultValue();
						var current = x.GetValue( target, null );

						var equalsDefault = current.As<string>().Transform( string.IsNullOrEmpty, () => Equals( current , defaultValue ) );
						var value = equalsDefault ? x.FromMetadata<System.ComponentModel.DefaultValueAttribute, object>( y => y.AsTo<DefaultAttribute, object>( z => z.GetValue( target, x ), () => y.Value  ) ) : null;
						var result = value.Transform( y => new { Property = x, Value = y } );
						return result;
					} )
					.NotNull()
					.Apply( item => item.Property.SetValue( target, item.Value, null ) );
			} ) );
		}
	}
}