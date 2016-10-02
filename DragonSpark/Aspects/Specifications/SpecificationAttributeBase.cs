using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace DragonSpark.Aspects.Specifications
{
	[ProvideAspectRole( KnownRoles.ParameterValidation ), LinesOfCodeAvoided( 1 ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation )]
	public abstract class SpecificationAttributeBase : ApplyInstanceAspectBase, ISpecification
	{
		protected SpecificationAttributeBase() : base( Support.Default ) {}

		ISpecification Specification { get; set; }
		public override void RuntimeInitializeInstance() => Specification = DetermineSpecification();
		protected abstract ISpecification DetermineSpecification();
		public bool IsSatisfiedBy( object parameter ) => Specification.IsSatisfiedBy( parameter );
	}
}