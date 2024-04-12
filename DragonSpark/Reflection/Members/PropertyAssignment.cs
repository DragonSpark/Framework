using DragonSpark.Model.Commands;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class PropertyAssignment<T, TValue> : Assign<T, TValue>
{
	protected PropertyAssignment(string name) : this(new PropertyDefinition<T>(name)) {}

	protected PropertyAssignment(PropertyInfo metadata)
		: base(PropertyAssignmentDelegates<T, TValue>.Default.Get(metadata)) {}
}