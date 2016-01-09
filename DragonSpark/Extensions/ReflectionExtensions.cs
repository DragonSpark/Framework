using Dynamitey;
using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class ReflectionExtensions
	{
		// public static object InvokeGeneric( this object @this, string methodName, params object[] parameters ) => InvokeGeneric( @this, methodName, parameters.Select( x => x.GetType() ).ToArray(), parameters );

		public static object InvokeGeneric( this object @this, string methodName, Type[] types, params object[] parameters ) => Invoke( @this.AsTo<Type, object>( InvokeContext.CreateStatic ) ?? @this, methodName, types, parameters );

		static object Invoke( object @this, string methodName, Type[] types, object[] parameters ) => Dynamic.InvokeMember( @this, InvokeMemberName.Create( methodName, types ), parameters );

		public static void InvokeGenericAction( this object @this, string methodName, Type[] types, params object[] parameters ) => InvokeAction( @this.AsTo<Type, object>( InvokeContext.CreateStatic ) ?? @this, methodName, types, parameters );

		static void InvokeAction( object @this, string methodName, Type[] types, object[] parameters ) => Dynamic.InvokeMemberAction( @this, InvokeMemberName.Create( methodName, types ), parameters );
	}
}