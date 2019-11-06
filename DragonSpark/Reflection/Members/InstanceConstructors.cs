using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members
{
	sealed class InstanceConstructors : Select<TypeInfo, IEnumerable<ConstructorInfo>>
	{
		public static InstanceConstructors Default { get; } = new InstanceConstructors();

		InstanceConstructors() : base(info => info.DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic)) {}
	}
}