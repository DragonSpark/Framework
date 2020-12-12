using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class PrincipalProperty<T, TValue> : Select<Memory<PropertyInfo>, PropertyInfo?>, IPrincipalProperty
	{
		public static PrincipalProperty<T, TValue> Default { get; } = new PrincipalProperty<T, TValue>();

		PrincipalProperty() : base(Start.An.Instance(LocateOnlyPrincipalProperty.Default)
		                                .Then()
		                                .OrMaybe(PrincipalPropertyByName<TValue>.Default)
		                                .Get()
		                                .To(x => new MultipleCandidatePrincipalProperty<T, TValue>(x))) {}
	}
}