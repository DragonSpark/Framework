using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Linq;

namespace DragonSpark.Reflection.Types;

public class GenericImplementationArguments : ISelect<Type, Array<Type>>
{
	readonly ISelect<Type, Array<Type>> _implementations;

	public GenericImplementationArguments(ISelect<Type, Array<Type>> implementations)
		=> _implementations = implementations;

	public Array<Type> Get(Type parameter) => _implementations.Get(parameter)
	                                                          .Open()
	                                                          .SelectMany(x => x.GenericTypeArguments)
	                                                          .ToArray();
}