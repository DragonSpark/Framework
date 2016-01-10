using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MetadataCustomization : AutoDataCustomization
	{
		public static MetadataCustomization Instance { get; } = new MetadataCustomization();

		readonly Func<MethodBase, ICustomization[]> factory;

		public MetadataCustomization() : this( MetadataCustomizationFactory.Instance.Create )
		{}

		public MetadataCustomization( Func<MethodBase, ICustomization[]> factory )
		{
			this.factory = factory;
		}

		protected override void OnInitializing( AutoData context ) => factory( context.Method ).Each( customization => customization.Customize( context.Fixture ) );
	}

	public class SetupCustomization<T> : AutoDataCustomization where T : class, ISetup
	{
		readonly Func<T> factory;

		public SetupCustomization() : this( ActivateFactory<T>.Instance.CreateUsing )
		{ }

		public SetupCustomization( Func<T> factory )
		{
			this.factory = factory;
		}

		protected override void OnInitializing( AutoData context )
		{
			using ( var arguments = new SetupParameter( context ) )
			{
				var setup = factory();
				setup.Run( arguments );
			}
		}

		public class SetupParameter : SetupParameter<AutoData>
		{
			public SetupParameter( AutoData arguments ) : base( arguments ) {}
		}
	}
}