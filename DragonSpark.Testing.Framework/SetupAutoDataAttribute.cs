using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Windows;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Testing.Framework
{
	public class Setup : Setup<RecordingLogger>
	{}

	public class SetupAutoDataContext
	{
		public SetupAutoDataContext( IFixture fixture, MethodInfo method )
		{
			Fixture = fixture;
			Method = method;
		}

		public IFixture Fixture { get; }
		public MethodInfo Method { get; }
	}

	public abstract class SetupAutoDataCommandBase : SetupCommand
	{
		protected override void Execute( SetupContext context )	
		{
			var setup = context.GetArguments<SetupAutoDataContext>();
			OnSetup( context, setup );
		}

		protected abstract void OnSetup( SetupContext context, SetupAutoDataContext setup );
	}

	[ContentProperty( nameof(Customizations) )]
	public class CustomizeFixtureCommand : SetupAutoDataCommandBase
	{
		public Collection<ICustomization> Customizations { get; } = new Collection<ICustomization>();

		protected override void OnSetup( SetupContext context, SetupAutoDataContext setup )
		{
			context.Container().EnsureRegistered( () => setup.Fixture );
			var customizations = new ICustomization[]
			{
				AmbientCustomizationsCustomization.Instance,
				new CurrentMethodCustomization( setup.Method ),
				new CompositeCustomization( Customizations ),
				new MetadataCustomization( setup.Method )
			};
			customizations.Apply( customization => setup.Fixture.Customize( customization ) );
		}
	}

	public class SetupAutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly ISetup setup;

		public SetupAutoDataAttribute() : this( new Fixture( DefaultEngineParts.Instance ) )
		{}

		public SetupAutoDataAttribute( IFixture fixture ) : this( fixture, typeof(DefaultSetup) )
		{}

		public SetupAutoDataAttribute( IFixture fixture, Type setupType ) : this( fixture, ActivateFactory<ISetup>.Instance.Locked( factory => factory.CreateUsing( setupType ) ) )
		{}

		public SetupAutoDataAttribute( IFixture fixture, ISetup setup ) : base( fixture )
		{
			this.setup = setup;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			setup.Run( new SetupAutoDataContext( Fixture, methodUnderTest ) );

			var result = base.GetData( methodUnderTest );
			return result;
		}
	}
}