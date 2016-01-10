using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using PostSharp.Aspects;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	[LinesOfCodeAvoided( 5 )]
	public class AutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute, IAspectProvider
	{
		public AutoDataAttribute() : this( FixtureFactory<AutoConfiguredMoqCustomization>.Instance.Create ) {}

		protected AutoDataAttribute( [Required]Func<IFixture> fixture ) : base( fixture() ) {}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			using ( new AssignExecutionContextCommand().ExecuteWith( MethodContext.Get( methodUnderTest ) ) )
			{
				using ( var autoData = new AutoData( Fixture, methodUnderTest ) )
				{
					using ( new AssignAutoDataCommand().ExecuteWith( autoData ) )
					{
						var result = base.GetData( methodUnderTest );
						return result;
					}
				}
			}
		}

		public IEnumerable<AspectInstance> ProvideAspects( object targetElement ) => targetElement.AsTo<MethodInfo, AspectInstance>( info => new AspectInstance( info, new AssignExecutionContextAspect() ) ).ToItem();
	}
}