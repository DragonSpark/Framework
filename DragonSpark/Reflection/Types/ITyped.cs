using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Types
{
	public interface ITyped<out T> : ISelect<Type, T> {}
}