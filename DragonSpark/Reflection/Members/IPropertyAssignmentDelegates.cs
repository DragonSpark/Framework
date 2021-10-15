using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public interface IPropertyAssignmentDelegate<in T> : ISelect<PropertyInfo, Action<object, T>> {}

public interface IPropertyAssignmentDelegate<in T, in TValue> : ISelect<PropertyInfo, Action<T, TValue>> {}