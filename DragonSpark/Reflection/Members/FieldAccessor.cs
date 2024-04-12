using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class FieldAccessor<T, TValue> : Select<T, TValue>
{
	protected FieldAccessor(string name) : this(new FieldDefinition<T>(name)) {}

	protected FieldAccessor(FieldInfo metadata) : base(FieldValueDelegates<T, TValue>.Default.Get(metadata)) {}
}

// TODO

public class PropertyAccessor<T, TValue> : Select<T, TValue>
{
	protected PropertyAccessor(string name) : this(new PropertyDefinition<T>(name)) {}

	protected PropertyAccessor(PropertyInfo metadata) : base(PropertyValueDelegates<T, TValue>.Default.Get(metadata)) {}
}
public class PropertyAssignment<T, TValue> : Assign<T, TValue>
{
	protected PropertyAssignment(string name) : this(new PropertyDefinition<T>(name)) {}

	protected PropertyAssignment(PropertyInfo metadata)
		: base(PropertyAssignmentDelegates<T, TValue>.Default.Get(metadata)) {}
}
