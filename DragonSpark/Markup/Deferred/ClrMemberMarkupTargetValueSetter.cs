using System;
using System.Diagnostics;
using System.Reflection;

namespace DragonSpark.Application.Markup.Deferred
{
    public sealed class ClrMemberMarkupTargetValueSetter<T> : IMarkupTargetValueSetter where T : MemberInfo
    {
        readonly object targetObject;
        readonly T targetProperty;
        readonly Action<object, T, object> assign;

        public ClrMemberMarkupTargetValueSetter( object targetObject, T targetProperty, Action<object, T, object> assign )
        {
            if ( targetProperty == null )
            {
                throw new ArgumentNullException( "targetProperty" );
            }

            this.targetObject = targetObject;
            this.targetProperty = targetProperty;
            this.assign = assign;
        }

        ~ClrMemberMarkupTargetValueSetter()
        {
            Debugger.Break();
        }

        public void SetValue( object value )
        {
            /*if ( targetProperty == null )
            {
                throw new ObjectDisposedException( GetType().FullName );
            }*/
	        assign( targetObject, targetProperty, value );

	        // targetProperty.SetValue( targetObject, value, null );
        }

        public void Dispose()
        {
            Debugger.Break();
            // GC.SuppressFinalize( this );
            // targetObject = null;
            // targetProperty = null;
        }
    }
}
