using PostSharp.Aspects.Advices;
using System;

namespace DragonSpark.Aspects.Specifications
{
	[IntroduceInterface( typeof(ISpecification) )]
	public class ApplySpecificationAttribute : SpecificationAttributeBase
	{
		readonly static Func<Type, ISpecification> Source = Specifications.Source.Default.Get;

		readonly Type specificationType;
		readonly Func<Type, ISpecification> source;

		public ApplySpecificationAttribute( Type specificationType ) : this( specificationType, Source ) {}

		protected ApplySpecificationAttribute( Type specificationType, Func<Type, ISpecification> source )
		{
			this.specificationType = specificationType;
			this.source = source;
		}

		protected override ISpecification DetermineSpecification() => source( specificationType );
	}
}