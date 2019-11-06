using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members
{
	interface IConstructors : ISelect<TypeInfo, ICollection<ConstructorInfo>> {}
}