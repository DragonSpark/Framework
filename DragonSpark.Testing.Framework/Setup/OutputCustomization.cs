using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Ploeh.AutoFixture;
using System.Linq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class OutputCustomization : CustomizationBase, ITestExecutionAware
	{
		[Activate]
		public IMessageLocator Locator { get; set; }

		protected override void Customize( IFixture fixture ) => new AssociatedSetup( fixture ).Item.Items.Add( this );

		void ITestExecutionAware.Before( SetupAutoData context )
		{}

		public void After( SetupAutoData context ) => new OutputValue( context.Method.DeclaringType ).Item.With( output =>
		{
			var messages = Locator.Create().OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
			messages.Each( output.WriteLine );
		} );
	}
}