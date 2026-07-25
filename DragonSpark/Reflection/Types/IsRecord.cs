using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DragonSpark.Reflection.Types;

public sealed class IsRecord : ICondition<Type>
{
	public static IsRecord Default { get; } = new();

	IsRecord() : this(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
	                  typeof(System.Text.StringBuilder)) {}

	readonly BindingFlags _flags;
	readonly Type[]       _types;

	public IsRecord(BindingFlags flags, params Type[] types)
	{
		_flags = flags;
		_types = types;
	}

	public bool Get(Type parameter)
		=> parameter.IsClass
			   ? parameter.GetProperty("EqualityContract", _flags)
			              ?.GetMethod?.Has<CompilerGeneratedAttribute>() == true
			   : parameter.IsValueType && parameter.GetMethod("PrintMembers", _flags, null, _types, null)
			                                       ?.Has<CompilerGeneratedAttribute>() == true;
}