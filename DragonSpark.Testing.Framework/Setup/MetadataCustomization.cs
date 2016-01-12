using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using System;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Diagnostics;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;

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

	public class SetupCustomization<TAssemblyProvider, TSetup> : AutoDataCustomization 
		where TAssemblyProvider : IAssemblyProvider
		where TSetup : class, ISetup
	{
		readonly Func<TSetup> factory;

		public SetupCustomization() : this( ActivateFactory<TSetup>.Instance.Create )
		{ }

		public SetupCustomization( Func<TSetup> factory )
		{
			this.factory = factory;
		}

		protected override void OnInitializing( AutoData context )
		{
			using ( var arguments = new SetupParameter<TAssemblyProvider>( context ) )
			{
				var setup = factory();
				setup.Run( arguments );
			}
		}

		public class SetupParameter<T> : ApplicationSetupParameter<AutoData> where T : IAssemblyProvider
		{
			public SetupParameter( AutoData arguments ) : base( new ServiceLocatorFactory<T, RecordingMessageLogger>().Create(), arguments ) {}
		}
	}
}