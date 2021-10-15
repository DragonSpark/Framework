using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Reflection.Types;

public class MakeGenericType : ISelect<Array<Type>, Type>, IActivateUsing<Type>
{
	readonly Type _definition;

	public MakeGenericType(Type definition) => _definition = definition;

	public Type Get(Array<Type> parameter) => _definition.MakeGenericType(parameter);
}