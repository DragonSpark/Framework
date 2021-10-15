using DragonSpark.Compose;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

sealed class PrincipalPropertyByName<T> : PrincipalPropertyByName
{
	public static PrincipalPropertyByName<T> Default { get; } = new PrincipalPropertyByName<T>();

	PrincipalPropertyByName() : base(A.Type<T>().Name) {}
}

class PrincipalPropertyByName : IPrincipalProperty
{
	readonly string _name;

	public PrincipalPropertyByName(string name) => _name = name;

	public PropertyInfo? Get(Memory<PropertyInfo> parameter)
	{
		foreach (var info in parameter.Span)
		{
			if (info.Name == _name)
			{
				return info;
			}
		}

		return default;
	}
}