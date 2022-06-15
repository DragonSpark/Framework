using DragonSpark.Model.Selection.Alterations;
using System;
using System.Reflection;

namespace DragonSpark.Reflection;

sealed class AccountForUnassignedType : IAlteration<Type>
{
	public static AccountForUnassignedType Default { get; } = new AccountForUnassignedType();

	AccountForUnassignedType() {}

	public Type Get(Type parameter) => Nullable.GetUnderlyingType(parameter)?.GetTypeInfo() ?? parameter;
}