using DragonSpark.Sources.Coercion;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class TypeCoercer : CoercerBase<Type>
	{
		public static TypeCoercer Default { get; } = new TypeCoercer();
		TypeCoercer() {}

		protected override Type Coerce( object parameter )
		{
			var info = parameter as ParameterInfo;
			if ( info != null )
			{
				return info.ParameterType;
			}

			var type = parameter as Type;
			if ( type != null )
			{
				return type;
			}

			var member = parameter as MemberInfo;
			var result = member?.GetMemberType() ?? parameter.GetType();
			return result;
		}
	}
}