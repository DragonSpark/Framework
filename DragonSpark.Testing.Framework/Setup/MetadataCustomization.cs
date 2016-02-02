using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
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


	public abstract class SetupAutoData : Setup<AutoData>
	{}

	public abstract class SetupCustomization<TSetup> : SetupCustomization where TSetup : class, ISetup<AutoData>
	{
		protected SetupCustomization() : base( ActivateFactory<TSetup>.Instance.Create ) {}
	}

	public abstract class SetupCustomization : AutoDataCustomization
	{
		readonly Func<ISetup<AutoData>> setupFactory;
	
		protected SetupCustomization( Func<ISetup<AutoData>> setupFactory )
		{
			this.setupFactory = setupFactory;
		}

		protected override void OnInitializing( AutoData context )
		{
			var setup = setupFactory();
			setup.Run( context );
		}

		/*public class SetupParameter : SetupParameter<AutoData>
		{
			public SetupParameter( IMessageLogger logger, AutoData arguments ) : base( logger, arguments ) {}
		}*/
	}
}