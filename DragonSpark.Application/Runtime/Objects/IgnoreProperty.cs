using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization.Metadata;

namespace DragonSpark.Application.Runtime.Objects;

public class IgnoreProperty : ICommand<JsonTypeInfo>
{
	readonly Type   _type;
	readonly string _name;

	protected IgnoreProperty(Type type, string name)
	{
		_type = type;
		_name = name;
	}

	public void Execute(JsonTypeInfo parameter)
	{
		if (parameter.Type == _type)
		{
			foreach (var property in parameter.Properties)
			{
				if (property.Name == _name)
				{
					property.ShouldSerialize = static (_, _) => false;
				}
			}
		}
	}
}

public class IgnoreProperty<T, TValue> : IgnoreProperty
{
	protected IgnoreProperty(Expression<Func<T, TValue>> name) : base(A.Type<T>(), name.GetMemberInfo().Name) {}
}