using DragonSpark.Model.Selection.Alterations;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	sealed class AccountForUnassignedType : IAlteration<TypeInfo>
	{
		public static AccountForUnassignedType Default { get; } = new AccountForUnassignedType();

		AccountForUnassignedType() {}

		public TypeInfo Get(TypeInfo parameter) => Nullable.GetUnderlyingType(parameter.AsType())
		                                                   ?.GetTypeInfo() ?? parameter;
	}
}