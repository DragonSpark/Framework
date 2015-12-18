using System;
using System.Windows.Markup;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Windows.Markup
{
	public interface IMarkupTargetValueSetter : IDisposable
	{
		void SetValue( object value );
	}

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

		protected sealed override IMarkupTargetValueSetter CreateItem( IProvideValueTarget parameter )
		{
			var result = Create( (TTarget)parameter.TargetObject, (TProperty)parameter.TargetProperty );
			return result;
		}

		protected abstract IMarkupTargetValueSetter Create( TTarget targetObject, TProperty targetProperty );

		protected abstract Type GetPropertyType( TTarget target, TProperty property );
	}
}
