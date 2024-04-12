using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class FieldDefinition<T> : FieldDefinition
{
	public FieldDefinition(string name) : base(A.Type<T>(), name) {}
}

public class FieldDefinition : Instance<FieldInfo>
{
	protected FieldDefinition(Type type, string name)
		: base(type.GetField(name, PrivateInstanceFlags.Default).Verify()) {}
}