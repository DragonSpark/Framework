using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Reflection.Types
{
	public sealed class IsNativeSystemType : Condition<Type>
	{
		public static IsNativeSystemType Default { get; } = new IsNativeSystemType();

		IsNativeSystemType() : base(x => x.Module.ScopeName.StartsWith("System.Private")) {}
	}
}
