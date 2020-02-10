using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	sealed class Constructors : Select<Type, ICollection<ConstructorInfo>>, IConstructors
	{
		public static Constructors Default { get; } = new Constructors();

		Constructors() : base(x => x.GetConstructors()) {}
	}
}