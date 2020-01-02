using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	public interface IAttributes<T> : IConditional<ICustomAttributeProvider, Array<T>> where T : Attribute {}
}