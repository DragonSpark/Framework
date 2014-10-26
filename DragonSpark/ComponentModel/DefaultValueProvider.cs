using DragonSpark.Extensions;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public interface IDefaultValueProvider
	{
		void Apply( object target );
	}

	class DefaultValueProvider : IDefaultValueProvider
	{
		public void Apply( object target )
		{
			target.GetType().GetRuntimeProperties()
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
		}
	}
}