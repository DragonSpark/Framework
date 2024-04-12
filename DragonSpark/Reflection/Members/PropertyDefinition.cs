using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class PropertyDefinition<T> : PropertyDefinition
{
	public PropertyDefinition(string name) : base(A.Type<T>(), name) {}
}
public class PropertyDefinition : Instance<PropertyInfo>
{
	protected PropertyDefinition(Type type, string name)
		: base(type.GetProperty(name, PrivateInstanceFlags.Default).Verify()) {}
}