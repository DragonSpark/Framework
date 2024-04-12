using DragonSpark.Model.Selection;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public class PropertyAccessor<T, TValue> : Select<T, TValue>
{
	protected PropertyAccessor(string name) : this(new PropertyDefinition<T>(name)) {}

	protected PropertyAccessor(PropertyInfo metadata) : base(PropertyValueDelegates<T, TValue>.Default.Get(metadata)) {}
}