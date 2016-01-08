using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
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
		readonly Func<Tuple<IFixture, MethodInfo>, IDisposable> factory;

		public AutoDataAttribute() : this( FixtureFactory<AutoConfiguredMoqCustomization>.Instance.Create )
		{}

		public AutoDataAttribute( Func<IFixture> fixture ) : this( fixture, AutoDataFactory.Instance.Create )
		{}

		protected AutoDataAttribute( [Required]Func<IFixture> fixture, Func<Tuple<IFixture, MethodInfo>, IDisposable> factory ) : base( fixture() )
		{
			this.factory = factory;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			using ( new AssignExecutionCommand().Apply( MethodContext.Get( methodUnderTest ) ) )
			{
				using ( factory( Tuple.Create( Fixture, methodUnderTest ) ) )
				{
					var result = base.GetData( methodUnderTest );
					return result;
				}
			}
		}

		public IEnumerable<AspectInstance> ProvideAspects( object targetElement ) => targetElement.AsTo<MethodInfo, AspectInstance>( info => new AspectInstance( info, new AssignExecutionAttribute() ) ).ToItem();
	}

	public class SetupParameter : SetupParameter<AutoData>
	{
		public SetupParameter( AutoData arguments ) : base( arguments )
		{}
	}
}