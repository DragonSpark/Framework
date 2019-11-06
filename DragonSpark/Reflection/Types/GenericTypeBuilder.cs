using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericTypeBuilder : ISelect<Type, Type>, IActivateUsing<Array<Type>>
	{
		readonly Array<Type> _parameters;

		public GenericTypeBuilder(Array<Type> parameters) => _parameters = parameters;

		public Type Get(Type parameter) => parameter.To(I<MakeGenericType>.Default).Get(_parameters);
	}
}