using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Runtime;
using Ploeh.AutoFixture.AutoMoq;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupFixtureFactory : FactoryBase<IFixture>
	{
		public static SetupFixtureFactory Instance { get; } = new SetupFixtureFactory();

		protected override IFixture CreateItem() => new Fixture( SetupEngineParts.Instance );
	}

	public class SetupAutoDataAttribute : AutoDataAttribute
	{
		protected SetupAutoDataAttribute( Func<AutoDataParameter, IEnumerable<object[]>> factory ) : this( SetupFixtureFactory.Instance.Create, factory )
		{}

		protected SetupAutoDataAttribute( Func<IFixture> fixture, Func<AutoDataParameter, IEnumerable<object[]>> factory ) : base( fixture, factory )
		{}
	}

	public class AutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly Func<AutoDataParameter, IEnumerable<object[]>> factory;

		public AutoDataAttribute() : this( FixtureFactory<AutoConfiguredMoqCustomization>.Instance.Create )
		{}

		public AutoDataAttribute( Func<IFixture> fixture ) : this( fixture, DelegatedAutoDataFactory.Instance.Create )
		{}

		protected AutoDataAttribute( Func<IFixture> fixture, Func<AutoDataParameter, IEnumerable<object[]>> factory ) : base( fixture() )
		{
			this.factory = factory;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			var data = SetupAutoData.Create( Fixture, methodUnderTest );
			var parameter = new AutoDataParameter( data, () => base.GetData( methodUnderTest ) );
			using ( var command = new AssignExecutionContextCommand() )
			{
				command.Execute( methodUnderTest );

				var result = factory( parameter );
				return result;
			}
		}
	}

	public class AutoDataParameter
	{
		readonly Func<IEnumerable<object[]>> @delegate;

		public AutoDataParameter( [Required]SetupAutoData data, [Required]Func<IEnumerable<object[]>> @delegate )
		{
			this.@delegate = @delegate;
			Data = data;
		}

		public SetupAutoData Data { get; }

		public IEnumerable<object[]> GetResult() => @delegate();
	}

	public class DelegatedAutoDataFactory : FactoryBase<AutoDataParameter, IEnumerable<object[]>>
	{
		public static DelegatedAutoDataFactory Instance { get; } = new DelegatedAutoDataFactory();

		protected override IEnumerable<object[]> CreateItem( AutoDataParameter parameter ) => parameter.GetResult();
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

		protected override IEnumerable<object[]> CreateItem( AutoDataParameter parameter )
		{
			using ( var arguments = new SetupParameter( parameter.Data ) )
			{
				var setup = factory();
				setup.Run( arguments );
				return base.CreateItem( parameter );
			}
		}
	}

	public class SetupParameter : SetupParameter<SetupAutoData>
	{
		public SetupParameter( SetupAutoData arguments ) : base( arguments )
		{}
	}
}