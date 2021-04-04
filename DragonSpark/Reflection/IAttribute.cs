using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	public interface IAttribute<out T> : IConditional<ICustomAttributeProvider, T?> where T : Attribute {}
}