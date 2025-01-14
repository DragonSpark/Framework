using System;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members;

public class PropertyAccessor<T, TValue> : Select<T, TValue>
{
	protected PropertyAccessor(string name) : this(new PropertyDefinition<T>(name)) {}

	protected PropertyAccessor(string name, BindingFlags flags) : this(new PropertyDefinition<T>(name, flags)) {}

	protected PropertyAccessor(Type type, string name, BindingFlags flags)
        : this(new PropertyDefinition(type, name, flags)) {}

	protected PropertyAccessor(PropertyInfo metadata) : base(PropertyValueDelegates<T, TValue>.Default.Get(metadata)) {}
}
public class PropertyAccessor : Select<object, object>
{
	protected PropertyAccessor(Type type, string name, BindingFlags flags)
        : this(new PropertyDefinition(type, name, flags)) {}

	protected PropertyAccessor(PropertyInfo metadata) : base(PropertyValueDelegates.Default.Get(metadata)) {}
}
