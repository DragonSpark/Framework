using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Ploeh.AutoFixture;
using System.Linq;
using DragonSpark.Aspects;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AutoDataCustomization : CustomizationBase, IAutoDataCustomization
	{
		protected override void Customize( IFixture fixture ) => new Items<IAutoDataCustomization>( fixture ).Item.Add( this );

		void IAutoDataCustomization.Initializing( AutoData data ) => OnInitializing( data );

		[BuildUp]
		protected virtual void OnInitializing( AutoData context ) {}

		void IAutoDataCustomization.Initialized( AutoData data ) => OnInitialized( data );

		[BuildUp]
		protected virtual void OnInitialized( AutoData context ) {}
	}

	public class OutputCustomization : AutoDataCustomization
	{
		[Activate]
		public IMessageLocator Locator { get; set; }

		protected override void OnInitialized( AutoData context )
		{
			var item = Locator.Create().OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
			new OutputValue( context.Method.DeclaringType ).Assign( item );
		}
	}
}