using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Reflection.Types;

sealed class GenericTypeBuilder : ISelect<Type, Type>, IActivateUsing<Array<Type>>
{
	readonly Array<Type> _parameters;

	public GenericTypeBuilder(Array<Type> parameters) => _parameters = parameters;

	public Type Get(Type parameter) => Start.An.Extent<MakeGenericType>().From(parameter).Get(_parameters);
}