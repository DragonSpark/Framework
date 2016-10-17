using System;
using System.Reflection;

namespace DragonSpark.Windows.Legacy.Markup
{
	public abstract class ClrMemberMarkupProperty<T> : MarkupPropertyBase where T : MemberInfo
	{
		readonly Action<object> setter;
		readonly Func<object> getter;

		protected ClrMemberMarkupProperty( T targetProperty, Action<object> setter, Func<object> getter ) : base( PropertyReference.New( targetProperty ) )
		{
			this.setter = setter;
			this.getter = getter;
		}

		protected override object OnGetValue() => getter();

		protected override object Apply( object value )
		{
			setter( value );
			return null;
		}
	}
}
