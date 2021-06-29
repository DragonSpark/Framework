using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Composition.Compose
{
	public interface IRelatedTypes : ISelect<Type, Lease<Type>> {}
}