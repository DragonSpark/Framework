using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Testing.Framework.Setup.Location;
using Ploeh.AutoFixture;
using System.Linq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AutoDataCustomization : CustomizationBase, IAutoDataCustomization
	{
		protected override void Customize( IFixture fixture )
		{
			var autoData = new CurrentAutoDataContext().Item.With( OnInitializing );
			var items = autoData?.Items ?? new Items<IAutoDataCustomization>( fixture ).Item;
			items.Ensure( ( IAutoDataCustomization)this );
		}

		void IAutoDataCustomization.Initializing( AutoData data ) => OnInitializing( data );

		protected virtual void OnInitializing( AutoData context ) {}

		void IAutoDataCustomization.Initialized( AutoData data ) => OnInitialized( data );

		protected virtual void OnInitialized( AutoData context ) {}
	}

	public class OutputCustomization : AutoDataCustomization
	{
		[Locate]
		public RecordingMessageLogger Logger { get; set; }

		[BuildUp]
		protected override void OnInitialized( AutoData context )
		{
			var item = Logger.Purge().OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
			new OutputValue( context.Method.DeclaringType ).Assign( item );
		}
	}
}