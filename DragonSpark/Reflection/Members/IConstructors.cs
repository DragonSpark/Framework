using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	interface IConstructors : ISelect<Type, ICollection<ConstructorInfo>> {}
}