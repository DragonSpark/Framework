using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public interface IFieldValueDelegate : IFieldValueDelegate<object> {}
public interface IFieldValueDelegate<out T> : IFieldValueDelegate<object, T> {}

public interface IFieldValueDelegate<in T, out TValue> : ISelect<FieldInfo, Func<T, TValue>> {}