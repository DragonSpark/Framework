using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members
{
	sealed class Constructors : Select<TypeInfo, ICollection<ConstructorInfo>>, IConstructors
	{
		public static Constructors Default { get; } = new Constructors();

		Constructors() : base(x => x.GetConstructors()) {}
	}
}