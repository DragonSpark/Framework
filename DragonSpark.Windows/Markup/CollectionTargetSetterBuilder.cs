using System;
using System.Collections;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public class CollectionTargetSetterBuilder : MarkupTargetValueSetterFactory<IList, object>
	{
		public static CollectionTargetSetterBuilder Instance { get; } = new CollectionTargetSetterBuilder();

		protected override bool Handles( IProvideValueTarget service )
		{
			return base.Handles( service ) && service.TargetObject.GetType().Adapt().GetInnerType() != null;
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
}