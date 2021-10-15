using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class ParameterType : Select<ParameterInfo, Type>
{
	public static ParameterType Default { get; } = new ParameterType();

	ParameterType() : base(x => x.ParameterType) {}
}