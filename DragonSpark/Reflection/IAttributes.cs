using System;
using System.Reflection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection
{
	public interface IAttributes<T> : IConditional<ICustomAttributeProvider, Array<T>> where T : Attribute {}
}