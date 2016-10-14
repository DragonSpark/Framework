using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using System;

namespace DragonSpark.Aspects.Alteration
{
	[IntroduceInterface( typeof(IAlteration) )]
	[ProvideAspectRole( KnownRoles.ValueConversion ), LinesOfCodeAvoided( 1 ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )]
	public sealed class AlterAttribute : ApplyInstanceAspectBase, IAlteration
	{
		readonly static Func<Type, IAlteration> Source = Aspects.Alteration.Source.Default.Get;
		readonly Type alterationType;

		public AlterAttribute( Type alterationType ) : base( Support.Default )
		{
			this.alterationType = alterationType;
		}

		IAlteration Alteration { get; set; }
		public override void RuntimeInitializeInstance() => Alteration = Source( alterationType );

		object IAlteration.Alter( object parameter ) => Alteration.Alter( parameter );
	}
}