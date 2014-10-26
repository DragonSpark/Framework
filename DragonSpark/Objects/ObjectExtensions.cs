using DragonSpark.Extensions;
using System.Linq;

namespace DragonSpark.Objects
{
	public static class ObjectExtensions
	{
		/*public static TItem ResolvedAs<TItem>( this object target, Func<object> source = null )
		{
			var resolvers = new Func<TItem>[]
				{
					() => target is TItem ? target.To<TItem>() : default(TItem),
					() => target.AsTo<ISingleton<TItem>, TItem>( x => x.Instance ),
					() => target.AsTo<IFactory, TItem>( x => x.Create<TItem>( source.Transform( y => y() ) ) )
				};

			var result = resolvers.Select( x => x() ).FirstOrDefault( x => !x.IsDefault() );
			return result;
		}*/

		public static TItem WithDefaults<TItem>( this TItem target ) where TItem : class 
		{
			target.GetType().GetProperties().Where( x => x.IsDecoratedWith<System.ComponentModel.DefaultValueAttribute>() ).Select( x =>
			{
			    var defaultValue = x.PropertyType.GetDefaultValue();
			    var current = x.GetValue( target, null );

				var equalsDefault = current.As<string>().Transform( string.IsNullOrEmpty, () => Equals( current , defaultValue ) );
			    var value = equalsDefault ? x.FromMetadata<System.ComponentModel.DefaultValueAttribute, object>( y => y.As<DefaultPropertyValueAttribute>().Transform( z => z.GetValue( target, x ), () => y.Value  ) ) : null;
			    var result = value.Transform( y => new { Property = x, Value = y } );
			    return result;
			} )
			.NotNull()
			.Apply( item => item.Property.SetValue( target, item.Value, null ) );
			return target;
		}
	}
}