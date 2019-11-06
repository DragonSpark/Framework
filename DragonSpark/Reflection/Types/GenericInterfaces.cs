using System;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericInterfaces : Store<Type, Array<Type>>
	{
		public static GenericInterfaces Default { get; } = new GenericInterfaces();

		GenericInterfaces() : base(AllInterfaces.Default.Query()
		                                        .Where(y => y.IsGenericType)
		                                        .Get) {}
	}
}