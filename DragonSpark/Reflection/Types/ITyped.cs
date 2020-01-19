using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Reflection.Types
{
	public interface ITyped<out T> : ISelect<Type, T> {}
}