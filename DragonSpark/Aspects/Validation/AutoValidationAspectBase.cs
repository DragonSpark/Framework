using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace DragonSpark.Aspects.Validation
{
	[LinesOfCodeAvoided( 4 ), ProvideAspectRole( KnownRoles.EnhancedValidation ), 
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, KnownRoles.ValueConversion ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, KnownRoles.ParameterValidation )
		]
	public abstract class AutoValidationAspectBase : AspectBase {}
}