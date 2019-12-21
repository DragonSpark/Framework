using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class AllInterfaces : Store<Type, Array<Type>>
	{
		public static AllInterfaces Default { get; } = new AllInterfaces();

		AllInterfaces() : base(TypeMetadata.Default.Select(Interfaces.Default)
		                                   .Query()
		                                   .Where(y => y.IsInterface)
		                                   .Distinct()
		                                   .Selector()) {}
	}
}