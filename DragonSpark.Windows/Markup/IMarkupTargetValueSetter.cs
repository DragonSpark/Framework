using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public interface IMarkupTargetValueSetter : IDisposable
	{
		void SetValue( object value );
	}

	public interface IMarkupTargetValueSetterBuilder : IFactory
	{
		bool Handles( IProvideValueTarget service );

		Type GetPropertyType( IProvideValueTarget parameter );
	}

	public abstract class MarkupTargetValueSetterFactory<TProperty> : MarkupTargetValueSetterFactory<object, TProperty>
	{}

	public abstract class MarkupTargetValueSetterFactory<TTarget, TProperty> : FactoryBase<IProvideValueTarget, IMarkupTargetValueSetter>, IMarkupTargetValueSetterBuilder
	{
		bool IMarkupTargetValueSetterBuilder.Handles( IProvideValueTarget service )
		{
			var result = Handles( service );
			return result;
		}

		public Type GetPropertyType( IProvideValueTarget parameter )
		{
			var result = GetPropertyType( (TTarget)parameter.TargetObject, (TProperty)parameter.TargetProperty );
			return result;
		}

		protected virtual bool Handles( IProvideValueTarget service )
		{
			var result = service.TargetObject is TTarget && ( typeof(TProperty) == typeof(object) || service.TargetProperty is TProperty );
			return result;
		}

		protected sealed override IMarkupTargetValueSetter CreateFrom( Type resultType, IProvideValueTarget parameter )
		{
			var result = Create( (TTarget)parameter.TargetObject, (TProperty)parameter.TargetProperty );
			return result;
		}

		protected abstract IMarkupTargetValueSetter Create( TTarget targetObject, TProperty targetProperty );

		protected abstract Type GetPropertyType( TTarget target, TProperty property );
	}

	public class CollectionTargetSetterBuilder : MarkupTargetValueSetterFactory<IList, object>
	{
		public static CollectionTargetSetterBuilder Instance { get; } = new CollectionTargetSetterBuilder();

		protected override bool Handles( IProvideValueTarget service )
		{
			return base.Handles( service ) && service.TargetObject.GetType().GetInnerType() != null;
		}

		protected override IMarkupTargetValueSetter Create( IList targetObject, object targetProperty )
		{
			var result = new CollectionSetter( targetObject );
			return result;
		}

		protected override Type GetPropertyType( IList target, object property )
		{
			return target.GetType();
		}
	}

	public class CollectionSetter : MarkupTargetValueSetterBase
	{
		readonly IList collection;
		
		public CollectionSetter( IList collection )
		{
			this.collection = collection;
		}

		protected override void Apply( object value )
		{
			var item = collection.Cast<object>().FirstOrDefault( o => o == null );
			var index = collection.IndexOf( item );
			collection.RemoveAt( index );

			var itemType = collection.GetType().GetInnerType();
			var items = itemType.IsInstanceOfType( value ) ? value.Append() : value is IEnumerable && itemType.IsAssignableFrom( value.GetType().GetInnerType() ) ? value.To<IEnumerable>().Cast<object>().Reverse() : Enumerable.Empty<object>();
			items.Apply( o => collection.Insert( index, o ) );
		}
	}

	public class DependencyPropertyMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<DependencyObject, DependencyProperty>
	{
		public static DependencyPropertyMarkupTargetValueSetterBuilder Instance { get; } = new DependencyPropertyMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( DependencyObject targetObject, DependencyProperty targetProperty )
		{
			var result = new DependencyPropertyMarkupTargetValueSetter( targetObject, targetProperty );
			return result;
		}

		protected override Type GetPropertyType( DependencyObject target, DependencyProperty property )
		{
			return property.PropertyType;
		}
	}

	public class PropertyInfoMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<PropertyInfo>
	{
		public static PropertyInfoMarkupTargetValueSetterBuilder Instance { get; } = new PropertyInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( object targetObject, PropertyInfo targetProperty )
		{
			return new ClrMemberMarkupTargetValueSetter<PropertyInfo>( targetObject, targetProperty, ( o, info, value ) => info.SetValue( o, value ) );
		}

		protected override Type GetPropertyType( object target, PropertyInfo property )
		{
			return property.PropertyType;
		}
	}

	public class FieldInfoMarkupTargetValueSetterBuilder : MarkupTargetValueSetterFactory<FieldInfo>
	{
		public static FieldInfoMarkupTargetValueSetterBuilder Instance { get; } = new FieldInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter Create( object targetObject, FieldInfo targetProperty )
		{
			return new ClrMemberMarkupTargetValueSetter<FieldInfo>( targetObject, targetProperty, ( o, info, value ) => info.SetValue( o, value ) );
		}

		protected override Type GetPropertyType( object target, FieldInfo property )
		{
			return property.FieldType;
		}
	}
}
