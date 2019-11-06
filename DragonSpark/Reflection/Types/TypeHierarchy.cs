using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection.Types
{
	public sealed class TypeHierarchy : ArrayStore<TypeInfo, TypeInfo>
	{
		public static TypeHierarchy Default { get; } = new TypeHierarchy();

		TypeHierarchy() : base(Hierarchy.Instance.Result().Get) {}

		sealed class Hierarchy : ISelect<TypeInfo, IEnumerable<TypeInfo>>
		{
			public static Hierarchy Instance { get; } = new Hierarchy();

			Hierarchy() {}

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
	}
}