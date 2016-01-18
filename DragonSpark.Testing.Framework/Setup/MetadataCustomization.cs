using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using System;
using System.Reflection;

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

	/*public class UnityContainerFactory<TAssemblyProvider> : UnityContainerFactory<TAssemblyProvider, RecordingMessageLogger> where TAssemblyProvider : IAssemblyProvider
	{
		public new static UnityContainerFactory<TAssemblyProvider> Instance { get; } = new UnityContainerFactory<TAssemblyProvider>();

		protected override IUnityContainer CreateItem() => base.CreateItem().Extension<FixtureExtension>().Container;
	}*/

	public abstract class SetupCustomization<TSetup> : SetupCustomization where TSetup : class, ISetup
	{
		protected SetupCustomization( Func<IServiceLocator> locatorFactory ) : base( locatorFactory, ActivateFactory<TSetup>.Instance.Create ) {}
	}

	public abstract class SetupCustomization : AutoDataCustomization
	{
		readonly Func<ISetup> setupFactory;
		readonly Func<IServiceLocator> locatorFactory;

		protected SetupCustomization( Func<IServiceLocator> locatorFactory, Func<ISetup> setupFactory )
		{
			this.locatorFactory = locatorFactory;
			this.setupFactory = setupFactory;
		}

		protected override void OnInitializing( AutoData context )
		{
			var locator = locatorFactory();
			using ( var arguments = new SetupParameter( locator, context ) )
			{
				var setup = setupFactory();
				setup.Run( arguments );
			}
		}

		public class SetupParameter : ApplicationSetupParameter<AutoData>
		{
			public SetupParameter( IServiceLocator locator, AutoData arguments ) : base( locator, arguments ) {}
		}
	}
}