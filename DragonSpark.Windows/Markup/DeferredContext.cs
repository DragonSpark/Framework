using System;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public class DeferredContext : IServiceProvider, IProvideValueTarget
	{
		readonly IServiceProvider inner;

		public DeferredContext( IServiceProvider inner, object targetObject, object targetProperty, Type propertyType )
		{
			this.inner = inner;
			TargetObject = targetObject;
			TargetProperty = targetProperty;
			PropertyType = propertyType;
		}

		public object TargetObject { get; }
		public object TargetProperty { get; }
		public Type PropertyType { get; }

		public virtual object GetService( Type serviceType )
		{
			var result =  new[] { typeof(DeferredContext), typeof(IProvideValueTarget) }.Any( type => type.IsAssignableFrom( serviceType ) ) ? this : inner.GetService( serviceType );
			return result;
		}
	}
}