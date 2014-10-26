using System;
using Dynamitey;

namespace DragonSpark.Extensions
{
	public static class ReflectionExtensions
	{
		public static void GenericInvoke( this Type @this, string methodName, Type[] types, params object[] parameters )
		{
			Invoke( InvokeContext.CreateStatic( @this ), methodName, types, parameters );
		}

		public static void GenericInvoke( this object @this, string methodName, Type[] types, params object[] parameters )
		{
			Invoke( @this, methodName, types, parameters );
		}

		static void Invoke( object @this, string methodName, Type[] types, object[] parameters )
		{
			Dynamic.InvokeMemberAction( @this, InvokeMemberName.Create( methodName, types ), parameters );
		}
	}
}