using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Reflection.Members;

public class PropertyDefinition<T> : PropertyDefinition
{
	public PropertyDefinition(string name, BindingFlags flags) : base(A.Type<T>(), name, flags) {}

	public PropertyDefinition(string name) : base(A.Type<T>(), name) {}
}
public class PropertyDefinition : Instance<PropertyInfo>
{
	public PropertyDefinition(Type type, string name) : this(type, name, PrivateInstanceFlags.Default) {}

	public PropertyDefinition(Type type, string name, BindingFlags flags) : base(type.GetProperty(name, flags).Verify()) {}
}
