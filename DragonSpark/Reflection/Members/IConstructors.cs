using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{
	interface IConstructors : ISelect<TypeInfo, ICollection<ConstructorInfo>> {}
}