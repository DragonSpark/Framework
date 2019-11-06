using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Application
{
	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.In(type)),
			       GenericTypeDefinition.Default.If(IsDefinedGenericType.Default)) {}
	}
}