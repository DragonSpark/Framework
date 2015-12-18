using System;
using System.Reflection;

namespace DragonSpark.Windows.Markup
{
	public sealed class ClrMemberMarkupTargetValueSetter<T> : MarkupTargetValueSetterBase where T : MemberInfo
	{
		readonly object targetObject;
		readonly T targetProperty;
		readonly Action<object, T, object> assign;

		public ClrMemberMarkupTargetValueSetter( object targetObject, T targetProperty, Action<object, T, object> assign )
		{
			if ( targetProperty == null )
			{
				throw new ArgumentNullException( nameof( targetProperty ) );
			}

			this.targetObject = targetObject;
			this.targetProperty = targetProperty;
			this.assign = assign;
		}

		protected override void Apply( object value )
		{
			assign( targetObject, targetProperty, value );
		}
	}
}
