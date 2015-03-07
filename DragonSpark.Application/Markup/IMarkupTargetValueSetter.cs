using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace DragonSpark.Application.Markup
{
	public interface IMarkupTargetValueSetter : IDisposable
	{
		void SetValue( object value );
	}

	public interface IMarkupTargetValueSetterBuilder : IFactory
	{}

	public class DependencyPropertyMarkupTargetValueSetterBuilder : FactoryBase<IProvideValueTarget, IMarkupTargetValueSetter>, IMarkupTargetValueSetterBuilder
	{
		public static DependencyPropertyMarkupTargetValueSetterBuilder Instance
		{
			get { return InstanceField; }
		}	static readonly DependencyPropertyMarkupTargetValueSetterBuilder InstanceField = new DependencyPropertyMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter CreateFrom( IProvideValueTarget parameter )
		{
			var dependencyObject = parameter.TargetObject as DependencyObject;
			var dependencyProperty = parameter.TargetProperty as DependencyProperty;

			var result = dependencyObject != null && dependencyProperty != null ? new DependencyPropertyMarkupTargetValueSetter( dependencyObject, dependencyProperty ) : null;
			return result;
		}
	}

	public class PropertyInfoMarkupTargetValueSetterBuilder : MemberInfoMarkupTargetValueSetterBuilder<PropertyInfo>, IMarkupTargetValueSetterBuilder
	{
		public static PropertyInfoMarkupTargetValueSetterBuilder Instance
		{
			get { return InstanceField; }
		}	static readonly PropertyInfoMarkupTargetValueSetterBuilder InstanceField = new PropertyInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter CreateSetter( IProvideValueTarget parameter, PropertyInfo member )
		{
			return new ClrMemberMarkupTargetValueSetter<PropertyInfo>( parameter.TargetObject, member, ( o, info, value ) => info.SetValue( o, value ) );
		}
	}

	public class FieldInfoMarkupTargetValueSetterBuilder : MemberInfoMarkupTargetValueSetterBuilder<FieldInfo>, IMarkupTargetValueSetterBuilder
	{
		public static FieldInfoMarkupTargetValueSetterBuilder Instance
		{
			get { return InstanceField; }
		}	static readonly FieldInfoMarkupTargetValueSetterBuilder InstanceField = new FieldInfoMarkupTargetValueSetterBuilder();

		protected override IMarkupTargetValueSetter CreateSetter( IProvideValueTarget parameter, FieldInfo member )
		{
			return new ClrMemberMarkupTargetValueSetter<FieldInfo>( parameter.TargetObject, member, ( o, info, value ) => info.SetValue( o, value ) );
		}
	}

	public abstract class MemberInfoMarkupTargetValueSetterBuilder<T> : FactoryBase<IProvideValueTarget, IMarkupTargetValueSetter> where T : MemberInfo
	{
		protected override IMarkupTargetValueSetter CreateFrom( IProvideValueTarget parameter )
		{
			var result = parameter.TargetProperty.AsTo<T, IMarkupTargetValueSetter>( member => CreateSetter( parameter, member ) );
			return result;
			/*var propInfo = parameter.TargetProperty as PropertyInfo;
			if ( propInfo != null )
			{
				return new ClrMemberMarkupTargetValueSetter( parameter.TargetObject, propInfo );
			}

			var fieldInfo = parameter.TargetProperty as FieldInfo;
			if ( fieldInfo != null )
			{
				return new ClrFieldMarkupTargetValueSetter( parameter.TargetObject, fieldInfo );
			}
			throw new NotImplementedException();*/
		}

		protected abstract IMarkupTargetValueSetter CreateSetter( IProvideValueTarget parameter, T member );
	}
}
