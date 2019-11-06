using System;
using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection
{
	public interface IAttribute<out T> : IConditional<ICustomAttributeProvider, T> where T : Attribute {}
}