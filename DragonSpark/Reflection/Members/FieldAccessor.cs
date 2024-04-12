using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class FieldAccessor<T, TValue> : Select<T, TValue>
{
	protected FieldAccessor(string name) : this(new FieldDefinition<T>(name)) {}

	protected FieldAccessor(FieldInfo metadata) : base(FieldValueDelegates<T, TValue>.Default.Get(metadata)) {}
}