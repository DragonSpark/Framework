using System;
using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.IoC
{
	public class IsRegisteredSpecification : SpecificationBase<Type>
	{
		readonly IUnityContainer container;

		public IsRegisteredSpecification( [Required]IUnityContainer container )
		{
			this.container = container;
		}

		protected override bool Verify( Type parameter ) => container.IsRegistered( parameter );
	}
}