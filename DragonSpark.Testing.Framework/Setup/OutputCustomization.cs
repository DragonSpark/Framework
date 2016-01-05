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

		public void Initialize( AutoData data ) => OnInitialize( data );

		[BuildUp]
		protected virtual void OnInitialize( AutoData data )
		{}

		void IAutoDataCustomization.Before( AutoData data ) => OnBefore( data );

		[BuildUp]
		protected virtual void OnBefore( AutoData context ) {}

		void IAutoDataCustomization.After( AutoData data ) => OnAfter( data );

		[BuildUp]
		protected virtual void OnAfter( AutoData context ) {}
	}

	public class OutputCustomization : AutoDataCustomization
	{
		[Activate]
		public IMessageLocator Locator { get; set; }

		protected override void OnAfter( AutoData context ) => new OutputValue( context.Method.DeclaringType ).Item.With( output =>
		{
			var messages = Locator.Create().OrderBy( line => line.Time ).Select( line => line.Text ).ToArray();
			messages.Each( output.WriteLine );
		} );
	}
}