using DragonSpark.Aspects.Build;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using System;

namespace DragonSpark.Aspects.Alteration
{
	[IntroduceInterface( typeof(IAlteration) )]
	[ProvideAspectRole( KnownRoles.ValueConversion ), LinesOfCodeAvoided( 1 ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )]
	public abstract class ApplyAlterationBase : ApplyInstanceAspectBase, IAlteration
	{
		readonly static Func<Type, IAlteration> Source = Aspects.Alteration.Source.Default.Get;

		readonly Type alterationType;

		protected ApplyAlterationBase( Type alterationType, SupportDefinitionBase source ) : base( source )
		{
			this.alterationType = alterationType;
		}

		IAlteration Alteration { get; set; }
		public override void RuntimeInitializeInstance() => Alteration = Source( alterationType );

		object IAlteration.Alter( object parameter ) => Alteration.Alter( parameter );
	}

	public sealed class ApplyAlterationAttribute : ApplyAlterationBase
	{
		public ApplyAlterationAttribute( Type alterationType ) : base( alterationType, Support<Aspect>.Default ) {}
	}

	public sealed class ApplyResultAlterationAttribute : ApplyAlterationBase
	{
		public ApplyResultAlterationAttribute( Type alterationType ) : base( alterationType, Support<ResultAspect>.Default ) {}
	}
}