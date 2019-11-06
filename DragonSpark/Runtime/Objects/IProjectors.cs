using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects
{
	public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}
}