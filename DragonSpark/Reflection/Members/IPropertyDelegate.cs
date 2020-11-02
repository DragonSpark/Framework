using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public interface IPropertyDelegate : IPropertyDelegate<object> {}

	public interface IPropertyDelegate<out T> : ISelect<PropertyInfo, Func<object, T>> {}
}