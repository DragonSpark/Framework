using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class InstanceConstructors : Select<TypeInfo, IEnumerable<ConstructorInfo>>
	{
		public static InstanceConstructors Default { get; } = new InstanceConstructors();

		InstanceConstructors() : base(info => info.DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic)) {}
	}
}