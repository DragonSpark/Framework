using DragonSpark.Extensions;
using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	public interface IMarkupProperty
	{
		PropertyReference Reference { get; }
		
		object GetValue();

		object SetValue( object value );
	}

	public abstract class MarkupPropertyFactoryBase<TTarget, TProperty> : MarkupPropertyFactoryBase
	{
		public sealed override IMarkupProperty Get( IServiceProvider parameter )
		{
			var target = parameter.Get<IProvideValueTarget>();
			var result = target != null ? Create( (TTarget)target.TargetObject, (TProperty)target.TargetProperty ) : null;
			return result;
		}

		protected abstract IMarkupProperty Create( TTarget targetObject, TProperty targetProperty );
	}
}
