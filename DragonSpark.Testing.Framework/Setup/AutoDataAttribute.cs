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
			using ( var command = new AssignExecutionCommand() )
			{
				command.Execute( MethodContext.Get( methodUnderTest ) );

				using ( factory( Tuple.Create( Fixture, methodUnderTest ) ) )
				{
					var result = base.GetData( methodUnderTest );
					return result;
				}
			}
		}

		public IEnumerable<AspectInstance> ProvideAspects( object targetElement ) => targetElement.AsTo<MethodInfo, AspectInstance>( info => new AspectInstance( info, new AssignExecutionAttribute() ) ).ToItem();
	}

	/*public class DelegatedAutoDataParameter
	{
		readonly Func<MethodInfo, IEnumerable<object[]>> @delegate;

		public DelegatedAutoDataParameter( [Required]AutoData data, [Required]Func<MethodInfo, IEnumerable<object[]>> @delegate )
		{
			this.@delegate = @delegate;
			Data = data;
		}

		public AutoData Data { get; }

		public IEnumerable<object[]> GetResult() => Data.Method.AsValid( @delegate );
	}*/

	/*public class DelegatedAutoDataFactory : FactoryBase<DelegatedAutoDataParameter, IEnumerable<object[]>>
	{
		public static DelegatedAutoDataFactory Instance { get; } = new DelegatedAutoDataFactory();

		protected override IEnumerable<object[]> CreateItem( DelegatedAutoDataParameter parameter ) => parameter.GetResult();
	}

	public class DelegatedSetupAutoDataFactory<T> : DelegatedAutoDataFactory where T : class, ISetup
	{
		public new static DelegatedSetupAutoDataFactory<T> Instance { get; } = new DelegatedSetupAutoDataFactory<T>();

		readonly Func<T> factory;

		public DelegatedSetupAutoDataFactory() : this( ActivateFactory<T>.Instance.CreateUsing )
		{}

		public DelegatedSetupAutoDataFactory( Func<T> factory )
		{
			this.factory = factory;
		}

		protected override IEnumerable<object[]> CreateItem( DelegatedAutoDataParameter parameter )
		{
			using ( var arguments = new SetupParameter( parameter.Data ) )
			{
				var setup = factory();
				setup.Run( arguments );
				return base.CreateItem( parameter );
			}
		}
	}*/

	public class SetupParameter : SetupParameter<AutoData>
	{
		public SetupParameter( AutoData arguments ) : base( arguments )
		{}
	}
}