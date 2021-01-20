using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	public class FieldDefinition<T> : FieldDefinition
	{
		public FieldDefinition(string name) : base(A.Type<T>(), name) {}
	}

	public class FieldDefinition : Instance<FieldInfo>
	{
		public FieldDefinition(Type type, string name)
			: base(type.GetField(name, PrivateInstanceFlags.Default).Verify()) {}
	}

	public class FieldAccessor<T, TValue> : Select<T, TValue>
	{
		public FieldAccessor(string name) : this(new FieldDefinition<T>(name)) {}

		public FieldAccessor(FieldInfo metadata) : base(FieldValueDelegates<T, TValue>.Default.Get(metadata)) {}
	}
}