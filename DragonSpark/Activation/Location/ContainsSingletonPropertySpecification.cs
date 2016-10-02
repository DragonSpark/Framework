using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Activation.Location
{
	public sealed class ContainsSingletonPropertySpecification : DelegatedAssignedSpecification<Type, PropertyInfo>
	{
		public static ContainsSingletonPropertySpecification Default { get; } = new ContainsSingletonPropertySpecification();
		ContainsSingletonPropertySpecification() : base( SingletonProperties.Default.ToSourceDelegate() ) {}
	}
}