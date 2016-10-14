using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using System;

namespace DragonSpark.Aspects.Coercion
{
	[IntroduceInterface( typeof(ICoercer) )]
	[ProvideAspectRole( KnownRoles.ValueConversion ), LinesOfCodeAvoided( 1 ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )]
	public sealed class ApplyCoercerAttribute : ApplyInstanceAspectBase, ICoercer
	{
		readonly static Func<Type, ICoercer> Source = Coercion.Source.Default.Get;
		readonly Type coercerType;

		public ApplyCoercerAttribute( Type coercerType ) : base( Support.Default )
		{
			this.coercerType = coercerType;
		}

		ICoercer Coercer { get; set; }
		public override void RuntimeInitializeInstance() => Coercer = Source( coercerType );

		object ICoercer.Coerce( object parameter ) => Coercer.Coerce( parameter );
	}
}