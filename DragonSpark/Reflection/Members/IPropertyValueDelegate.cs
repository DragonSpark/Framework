using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public interface IPropertyValueDelegate : IPropertyValueDelegate<object> {}
public interface IPropertyValueDelegate<out T> : IPropertyValueDelegate<object, T> {}

public interface IPropertyValueDelegate<in T, out TValue> : ISelect<PropertyInfo, Func<T, TValue>> {}