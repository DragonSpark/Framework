using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Reflection.Members;

public interface IPropertyDelegates : ISelect<(Type Owner, string Name), Func<object, object?>?>;

public interface IPropertyDelegates<out T> : ISelect<(Type Owner, string Name), Func<object, T>>;