using System;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	public class MarkupServiceProvider : IServiceProvider, IProvideValueTarget
	{
		readonly static Type[] Types = { typeof(MarkupServiceProvider), typeof(IProvideValueTarget) };

		readonly IServiceProvider inner;

		public MarkupServiceProvider( IServiceProvider inner, object target, IMarkupProperty property )
		{
			this.inner = inner;
			TargetObject = target;
			Property = property;
		}

		public object TargetObject { get; }

		public IMarkupProperty Property { get; }

		object IProvideValueTarget.TargetProperty => Property;
		
		public virtual object GetService( Type serviceType ) => Types.Any( type => type.IsAssignableFrom( serviceType ) ) ? this : inner.GetService( serviceType );
	}
}