using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public interface IPropertyDelegates : IPropertyDelegates<object> {}

	public interface IPropertyDelegates<out T> : ISelect<Type, Func<PropertyInfo, Func<object, T>>> {}
}