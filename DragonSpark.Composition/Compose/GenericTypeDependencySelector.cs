using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Composition.Compose
{
	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.Get(type)),
			       GenericTypeDefinition.Default.Then()
			                            .Ensure.Input.Is(IsDefinedGenericType.Default)
			                            .Otherwise.Use(x => x)) {}
	}
}