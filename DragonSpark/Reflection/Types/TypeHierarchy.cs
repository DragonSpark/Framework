using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

sealed class TypeHierarchy : ISelect<TypeInfo, IEnumerable<TypeInfo>>
{
	public static TypeHierarchy Default { get; } = new TypeHierarchy();

	TypeHierarchy() {}

	public IEnumerable<TypeInfo> Get(TypeInfo parameter)
	{
		yield return parameter;
		var current = parameter.BaseType;
		while (current != null)
		{
			var info = current.GetTypeInfo();
			if (current != typeof(object))
			{
				yield return info;
			}

			current = info.BaseType;
		}
	}
}